using BookingService.Data;
using BookingService.Model.Entity;
using BookingService.Repository.Contract;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Repository.Implementation;

public class ReviewRepository(AppDbContext context): CrudRepository<Review>(context), IReviewRepository
{
    public async Task<Review?> GetByEntityInfoRaterUsername(string entityInfo, string raterUsername) =>
        await _dbSet.FirstOrDefaultAsync(a =>
                a.RaterUsername == raterUsername &&
                a.EntityInfo.Equals(entityInfo, StringComparison.CurrentCultureIgnoreCase)
        );
}