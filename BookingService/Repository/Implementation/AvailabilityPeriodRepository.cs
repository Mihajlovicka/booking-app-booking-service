using BookingService.Data;
using BookingService.Model.Entity;
using BookingService.Repository.Contract;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Repository.Implementation;

public class AvailabilityPeriodRepository(AppDbContext context)
    : CrudRepository<AvailabilityPeriod>(context), IAvailabilityPeriodRepository
{

    public async Task<IEnumerable<AvailabilityPeriod>> GetByAccommodation(string accommodationId, bool? fromToday = null)
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

    // public bool Overlaps(string accommodationId, int? periodId, DateTime start, DateTime end)
    // {
    //     return _dbSet.Include(p => p.Accommodation).Any(p =>
    //         p.Accommodation.ExternalId == accommodationId &&
    //         p.StartDate < end && p.EndDate > start &&
    //         (p.Id != periodId)
    //     );
    // }

    public bool Overlaps(string accommodationId, int? periodId, DateTime start, DateTime end)
    {
        // Step 1: check if any periods overlap
        var hasOverlappingPeriod = _dbSet
            .Include(p => p.Accommodation)
            .Any(p =>
                p.Accommodation.ExternalId == accommodationId &&
                p.StartDate < end &&
                p.EndDate > start &&
                p.Id != periodId
            );

        if (!hasOverlappingPeriod)
            return false;

        // Step 2: check if there are active reservations in that time range
        var hasActiveReservation = _dbSet
            .Select(p => p.Accommodation)
            .Where(a => a.ExternalId == accommodationId)
            .SelectMany(a => a.Reservations)
            .Any(r =>
                r.StartDate < end &&
                r.EndDate > start &&
                r.Active
            );

        // If there’s an active reservation, you cannot change the period
        if (hasActiveReservation)
            return true;

        return hasOverlappingPeriod;
    }
}