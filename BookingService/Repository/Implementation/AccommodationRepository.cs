using BookingService.Data;
using BookingService.Repository.Contract;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Repository.Implementation;

public class AccommodationRepository(AppDbContext context)
    : CrudRepository<Accommodation>(context), IAccommodationRepository
{
    public async Task<Accommodation?> GetByExternalIdAsync(string externalId)
    {
        return await _dbSet
            .FirstOrDefaultAsync(a => a.ExternalId == externalId);
    }

}