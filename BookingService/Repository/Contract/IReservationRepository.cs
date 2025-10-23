using BookingService.Model.Entity;

namespace BookingService.Repository.Contract;

public interface IReservationRepository : ICrudRepository<Reservation>
{
    Task<List<Reservation>> GetAllReservationsForAccommodation(string accommodationId);
    bool Overlaps(string accommodationId, DateTime start, DateTime end);
    Task<List<Reservation>> GetMy(string username);
}