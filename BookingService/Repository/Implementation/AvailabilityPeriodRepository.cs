using BookingService.Data;
using BookingService.Model.Entity;
using BookingService.Repository.Contract;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Repository.Implementation;

public class AvailabilityPeriodRepository(AppDbContext context)
    : CrudRepository<AvailabilityPeriod>(context), IAvailabilityPeriodRepository
{

    public async Task<IEnumerable<AvailabilityPeriod>> GetByAccommodation(string accommodationId, bool? fromToday=null)
    {
        var query = _dbSet
            .Include(p => p.Accommodation)
            .Where(p => p.Accommodation.ExternalId == accommodationId);

        if (fromToday != true) return await query.ToListAsync();
        {
            var today = DateTime.UtcNow.Date;
            query = query.Where(p => p.StartDate.Date >= today);
        }

        return await query.ToListAsync();
    }

    public bool Overlaps(string accommodationId, int? periodId, DateTime start, DateTime end)
    {
        return  _dbSet.Include(p => p.Accommodation).Any(p =>
            p.Accommodation.ExternalId == accommodationId &&
            p.StartDate < end && p.EndDate > start &&
            (p.Id != periodId)
        );
    }
}