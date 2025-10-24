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

    public async Task<IEnumerable<Review>> GetByEntityInfo(string entityInfo)
    {
        return await _dbSet.Where(a =>
            a.EntityInfo.Equals(entityInfo, StringComparison.CurrentCultureIgnoreCase)
        ).ToListAsync();
    }
    
    public async Task DeleteTwoReviewsAsync(int firstReviewId, int secondReviewId)
    {
        var firstReview = await _dbSet.FindAsync(firstReviewId);
        var secondReview = await _dbSet.FindAsync(secondReviewId);

        if (firstReview != null)
            _dbSet.Remove(firstReview);

        if (secondReview != null)
            _dbSet.Remove(secondReview);

        await _context.SaveChangesAsync();
    }
}