using BookingService.Mapper;
using BookingService.Model.Dto;
using BookingService.Model.Entity;
using BookingService.Repository.Contract;
using BookingService.Service.Contract;

namespace BookingService.Service.Implementation;

public class ReviewService(IRepositoryManager repositoryManager) : IReviewService
{
    public async Task Add(IEnumerable<ReviewDto> reviewDtos)
    {
        foreach (var reviewDto in reviewDtos)
        {
            var user = await repositoryManager.UserRepository.GetByUsernameAsync(reviewDto.RaterUsername);
            if (user is null)
                throw new Exception("User does not exist");
            
            var review =
                await repositoryManager.ReviewRepository.GetByEntityInfoRaterUsername(reviewDto.EntityInfo,
                    reviewDto.RaterUsername ?? "");

            if (review is null)
            {
                review = new Review()
                {
                    RaterUsername = reviewDto.RaterUsername,
                    Grade = reviewDto.Grade,
                    ReviewFor = Enum.Parse<ReviewFor>(reviewDto.ReviewFor, true)
                };
                
                switch (reviewDto.ReviewFor.ToUpperInvariant())
                {
                    case "HOST":
                    {
                        var host = await repositoryManager.UserRepository.GetByUsernameAsync(reviewDto.EntityInfo);
                        if (host is null)
                            throw new Exception($"Host '{reviewDto.EntityInfo}' does not exist");
                        review.EntityInfo = host.Username;
                        break;
                    }
                    case "ACCOMMODATION":
                    {
                        if (!Guid.TryParse(reviewDto.EntityInfo, out var externalId))
                            throw new Exception($"Invalid accommodation ID: {reviewDto.EntityInfo}");
    
                        var accommodation =
                            await repositoryManager.AccommodationRepository.GetByExternalIdAsync(reviewDto.EntityInfo);
                        if (accommodation is null)
                            throw new Exception($"Accommodation '{reviewDto.EntityInfo}' does not exist");
                        review.EntityInfo = accommodation.ExternalId;
                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException(nameof(reviewDto.ReviewFor),
                            $"Invalid ReviewFor value: {reviewDto.ReviewFor}");
                }
                
                await repositoryManager.ReviewRepository.AddAsync(review);
            }

            review.Grade = reviewDto.Grade;
            
            await repositoryManager.ReviewRepository.UpdateAsync(review);
        }
    }

}