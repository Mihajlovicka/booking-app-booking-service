using BookingService.Mapper;
using BookingService.Model.Dto;
using BookingService.Model.Entity;
using BookingService.Repository.Contract;
using BookingService.Service.Contract;

namespace BookingService.Service.Implementation;

public class ReservationRequestService(IMapperManager mapperManager, IRepositoryManager repositoryManager, IReservationService reservationService) : IReservationRequestService
{
    public async Task<ReservationRequestDto> Add(string guestUsername, CreateReservationRequestDto dto)
    {
        var reservationRequest = await mapperManager.CreateReservationRequestDtoToReservationRequestMapper.Map(dto);

        var accommodation =
            await repositoryManager.AccommodationRepository.GetByExternalIdAsync(dto.AccommodationExternalId);

        if (accommodation is null) throw new Exception("Accommodation does not exist!");

        reservationRequest.Accommodation = accommodation;

        if (reservationRequest.GuestNumber == 0 || !IsGuestNumInValidLimit(reservationRequest.GuestNumber, accommodation.MinNumberOfGuests,
                accommodation.MaxNumberOfGuests))
        {
            throw new Exception("Guest number is not valid for defined limit");
        }

        var guest = await repositoryManager.UserRepository.GetByUsernameAsync(guestUsername);

        if (guest is null) throw new Exception("User does not exist");
        
        if (!guest.Role.Equals(Role.GUEST)) throw new Exception("Host cannot request reservation");
        
        reservationRequest.Guest = guest;
        
        var availabilityPeriods = await repositoryManager.AvailabilityPeriodRepository
            .GetByAccommodation(dto.AccommodationExternalId);

        var availabilityPeriod = availabilityPeriods.FirstOrDefault(ap =>
            ap.StartDate <= reservationRequest.StartDate && ap.EndDate >= reservationRequest.EndDate);

        if (availabilityPeriod is null) throw new Exception("Requested period is not available for this accommodation.");
        
        reservationRequest.FinalPrice = GetFinalPrice(accommodation, availabilityPeriod, dto.GuestNumber);

        await repositoryManager.ReservationRequestRepository.AddAsync(reservationRequest);

        if (reservationRequest.Accommodation.AutomaticReservation)
        {
            await AcceptRequest(reservationRequest.ExternalId.ToString());
        }

        return await mapperManager.ReservationRequestToReservationRequestDtoMapper.Map(reservationRequest);
    }

    public async Task<IEnumerable<ReservationRequestDto>> GetAllForAccommodation(string accommodationId)
    {
        var reservationRequests =
            await repositoryManager.ReservationRequestRepository.GetAllForAccommodation(accommodationId);

        var mappedRequests = new List<ReservationRequestDto>();

        foreach (var request in reservationRequests)
        {
            var mapped = await mapperManager.ReservationRequestToReservationRequestDtoMapper.Map(request);
            mappedRequests.Add(mapped);
        }

        return mappedRequests;
    }

    public async Task RejectRequest(string requestExternalId)
    {
        if (!Guid.TryParse(requestExternalId, out var requestGuid))
            throw new Exception("Invalid reservation request ID");

        var reservationRequest = await repositoryManager.ReservationRequestRepository.GerByExternalId(requestGuid);

        if (reservationRequest == null) 
            throw new Exception("Request not found");

        await repositoryManager.ReservationRequestRepository.DeleteAsync(reservationRequest.Id);
    }

    public async Task AcceptRequest(string requestExternalId)
    {
        if (!Guid.TryParse(requestExternalId, out var requestGuid))
            throw new Exception("Invalid reservation request ID");

        var reservationRequest = await repositoryManager.ReservationRequestRepository.GerByExternalId(requestGuid);

        if (reservationRequest == null) 
            throw new Exception("Request not found");
        
        var requestsForDeletion = await repositoryManager.ReservationRequestRepository.Overlaps(
            reservationRequest.Accommodation.ExternalId, reservationRequest.StartDate, reservationRequest.EndDate);

        foreach (var request in requestsForDeletion)
        {
            await repositoryManager.ReservationRequestRepository.DeleteAsync(request.Id);
        }
        
        await reservationService.CreateReservation(reservationRequest);

        await repositoryManager.ReservationRequestRepository.DeleteAsync(reservationRequest.Id);
    }

    private static decimal GetFinalPrice(Accommodation accommodation, AvailabilityPeriod availabilityPeriod, int guestNum)
    {
        var numOfDays = GetNumOfDays(availabilityPeriod.StartDate, availabilityPeriod.EndDate);
        var res = accommodation.PriceType switch
        {
            PriceType.PerGuest => guestNum * availabilityPeriod.Price * numOfDays,
            PriceType.PerUnit => availabilityPeriod.Price * numOfDays,
            _ => 0
        };

        return res;
    }

    private static int GetNumOfDays(DateTime start, DateTime end)
    {
        return (end - start).Days + 1;
    }

    private static bool IsGuestNumInValidLimit(int guestNumber, int? minLimit, int? maxLimit)
    {
        return minLimit <= guestNumber && guestNumber <= maxLimit;
    }
}