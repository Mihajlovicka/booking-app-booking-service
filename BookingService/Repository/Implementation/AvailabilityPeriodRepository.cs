using System.Linq.Expressions;
using System.Threading.Tasks;
using BookingService.Data;
using BookingService.Model.Entity;
using BookingService.Repository.Contract;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Repository.Implementation;

public class AvailabilityPeriodRepository(AppDbContext context)
    : CrudRepository<AvailabilityPeriod>(context), IAvailabilityPeriodRepository
{

    public async Task<IEnumerable<AvailabilityPeriod>> GetByAccommodation(string accommodationId)
    {
         return await _dbSet
            .Include(p => p.Accommodation)
            .Where(p => p.Accommodation.ExternalId == accommodationId)
            .ToListAsync();
    }

    public bool Overlaps(string accommodationId, int? periodId, DateTime start, DateTime end, int? excludeId = null)
    {
        return  _dbSet.Include(p => p.Accommodation).Any(p =>
            p.Accommodation.ExternalId == accommodationId &&
            (!excludeId.HasValue || p.Id != excludeId) &&
            p.StartDate < end && p.EndDate > start &&
            (p.Id != periodId)
        );
    }

}