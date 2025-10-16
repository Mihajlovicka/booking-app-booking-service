using BookingService.Mapper;
using BookingService.Model.Dto;
using BookingService.Model.Entity;
using BookingService.Repository.Contract;
using BookingService.Service.Contract;

namespace BookingService.Service.Implementation;

public class ReservationRequestService(IMapperManager mapperManager, IRepositoryManager repositoryManager) : IReservationRequestService
{
    public async Task<ReservationRequestDto> Add(string guestUsername, CreateReservationRequestDto dto)
    {
        var reservationRequest = mapperManager.CreateReservationRequestDtoToReservationRequestMapper.Map(dto);

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

        return mapperManager.ReservationRequestToReservationRequestDtoMapper.Map(reservationRequest);
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