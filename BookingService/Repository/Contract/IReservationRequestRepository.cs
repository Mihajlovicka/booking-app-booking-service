using BookingService.Model.Entity;

namespace BookingService.Repository.Contract;

public interface IReservationRequestRepository : ICrudRepository<ReservationRequest>
{
    Task<IEnumerable<ReservationRequest>> GetAllForAccommodation(string accommodationId);
    Task<ReservationRequest?> GerByExternalId(Guid externalId);
    Task<IEnumerable<ReservationRequest>> Overlaps(string accommodationId, DateTime start, DateTime end);
}