using BookingService.Model.Entity;

namespace BookingService.Repository.Contract;

public interface IReservationRepository : ICrudRepository<Reservation>
{
    Task<List<Reservation>> GetAllReservationsForAccommodation(string accommodationId);
}