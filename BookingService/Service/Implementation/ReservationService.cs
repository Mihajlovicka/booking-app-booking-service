using BookingService.Mapper;
using BookingService.Model.Dto;
using BookingService.Model.Entity;
using BookingService.Repository.Contract;
using BookingService.Service.Contract;

namespace BookingService.Service.Implementation;

public class ReservationService(IMapperManager mapperManager, IRepositoryManager repositoryManager)
    : IReservationService
{
    public async Task<IEnumerable<ReservationDto>> GetByAccommodation(string accommodationId)
    {
        var reservations =
            await repositoryManager.ReservationRepository.GetAllReservationsForAccommodation(accommodationId);
        return await Task.WhenAll(reservations.Select(p => mapperManager.ReservationToReservationDtoMapper.Map(p)));
    }

    public async Task CreateReservation(ReservationRequest request)
    {
        var accommodation =
            await repositoryManager.AccommodationRepository.GetByExternalIdAsync(request.Accommodation.ExternalId);

        if (accommodation is null) throw new Exception("Accommodation does not exists");

        if (repositoryManager.ReservationRepository.Overlaps(accommodation.ExternalId, request.StartDate,
                request.EndDate))
        {
            throw new Exception("For this period reservation already exists!");
        }

        Reservation reservation = new()
        {
            FinalPrice = request.FinalPrice,
            GuestNumber = request.GuestNumber,
            GuestUsername = request.Guest.Username,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Accommodation = accommodation,
            ExternalId = Guid.NewGuid()
        };

        await repositoryManager.ReservationRepository.AddAsync(reservation);
    }
}