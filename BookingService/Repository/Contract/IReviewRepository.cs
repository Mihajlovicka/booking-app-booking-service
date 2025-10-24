using BookingService.Model.Entity;

namespace BookingService.Repository.Contract;

public interface IReviewRepository : ICrudRepository<Review>
{
    Task<Review?> GetByEntityInfoRaterUsername(string entityInfo, string raterUsername);
    Task<IEnumerable<Review>> GetByEntityInfo(string entityInfo);
    Task DeleteTwoReviewsAsync(int firstReviewId, int secondReviewId);
}
