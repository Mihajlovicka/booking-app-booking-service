using BookingService.Model.Entity;

namespace BookingService.Repository.Contract;

public interface IAvailabilityPeriodRepository : ICrudRepository<AvailabilityPeriod>
{
    Task<IEnumerable<AvailabilityPeriod>> GetByAccommodation(string accommodationId, bool? fromToday=null);
    bool Overlaps(string accommodationId, int? periodId, DateTime start, DateTime end);
}