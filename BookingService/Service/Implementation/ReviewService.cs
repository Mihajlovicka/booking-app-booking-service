using BookingService.Mapper;
using BookingService.Model.Dto;
using BookingService.Model.Entity;
using BookingService.Model.Messages;
using BookingService.Repository.Contract;
using BookingService.Service.Contract;
using BookingService.Service.MessagingService;

namespace BookingService.Service.Implementation;

public class ReviewService(IRepositoryManager repositoryManager,
    ProducerService producerService) : IReviewService
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

                NotificationDto notificationDto;
                
                switch (reviewDto.ReviewFor.ToUpperInvariant())
                {
                    case "HOST":
                    {
                        var host = await repositoryManager.UserRepository.GetByUsernameAsync(reviewDto.EntityInfo);
                        if (host is null)
                            throw new Exception($"Host '{reviewDto.EntityInfo}' does not exist");
                            review.EntityInfo = host.Username;
                        

                            user = await repositoryManager.UserRepository.GetByUsernameAsync(host.Username);

                            notificationDto = new NotificationDto
                            {
                                NotificationTypeId = (int)NotificationType.NewRateOnHost,
                                NotificationUserExternalId = user.ExternalId,
                                Message = "New rating on you"
                            };
                            await producerService.ProduceAsync<NotificationDto>(KafkaTopic.NotificationCreated.ToString(), notificationDto);
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
                        

                        user = await repositoryManager.UserRepository.GetByUsernameAsync(accommodation.Owner);

                            notificationDto = new NotificationDto
                            {
                                NotificationTypeId = (int)NotificationType.NewRateOnAccommodation,
                                NotificationUserExternalId = user.ExternalId,
                                Message = "New rating on accomodation " + accommodation.Name
                            };
                            await producerService.ProduceAsync<NotificationDto>(KafkaTopic.NotificationCreated.ToString(), notificationDto);
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

    public async Task<double> GetAverageGradeForAccommodation(string externalId)
    {
        var grades = (await repositoryManager.ReviewRepository
                .GetByEntityInfo(externalId))
            .Select(x => x.Grade);

        var enumerable = grades as int[] ?? grades.ToArray();
        var averageGrade = enumerable.Length == 0 ? 0 : enumerable.Average();

        return averageGrade;
    }

    public async Task<IEnumerable<ViewHostAccommodationReviewDto>> GetAccommodationViewGrades(string externalId)
    {
        var accommodation = await repositoryManager.AccommodationRepository.GetByExternalIdAsync(externalId);

        var ownerUsername = accommodation.Owner;

        List<ViewHostAccommodationReviewDto> result = [];
        var accommodationReviews = await repositoryManager.ReviewRepository.GetByEntityInfo(externalId);
        
        foreach (var rate in accommodationReviews)
        {
            var hostinfo = await GetHostReviewInfoRaterAndHost(rate.RaterUsername, ownerUsername);
            result.Add(new ViewHostAccommodationReviewDto()
            {
                RaterUserName = rate.RaterUsername,
                AccommodationGrade = rate.Grade,
                AccommodationName = accommodation.Name,
                AccommodationExternalId = externalId,
                HostUsername = hostinfo?.EntityInfo ?? "",
                HostGrade = hostinfo?.Grade ?? 0
            });
        }

        return result;
    }
    
    public async Task<ViewHostAccommodationReviewDto> GetAccommodationViewGradesForRater(string externalId, string raterUsername)
    {
        var accommodation = await repositoryManager.AccommodationRepository.GetByExternalIdAsync(externalId);

        var ownerUsername = accommodation.Owner;

        var hostReview = await GetHostReviewInfoRaterAndHost(raterUsername, ownerUsername);
        
        var accReview =  await repositoryManager.ReviewRepository.GetByEntityInfoRaterUsername(externalId, raterUsername);

        return new ViewHostAccommodationReviewDto()
        {
            RaterUserName = raterUsername,
            AccommodationGrade = accReview?.Grade ?? 0,
            AccommodationName = accommodation.Name,
            AccommodationExternalId = externalId,
            HostUsername = ownerUsername,
            HostGrade = hostReview?.Grade ?? 0
        };
    }

    public async Task DeleteReviewsForAccommodation(string externalId, string raterUsername)
    {
        var accommodation = await repositoryManager.AccommodationRepository.GetByExternalIdAsync(externalId);

        var ownerUsername = accommodation.Owner;

        var hostReview = await GetHostReviewInfoRaterAndHost(raterUsername, ownerUsername);
        
        var accReview =  await repositoryManager.ReviewRepository.GetByEntityInfoRaterUsername(externalId, raterUsername);

        await repositoryManager.ReviewRepository.DeleteTwoReviewsAsync(hostReview.Id, accReview.Id);
    }

    private async Task<Review?> GetHostReviewInfoRaterAndHost(string rater, string host)
    {
        return await repositoryManager.ReviewRepository.GetByEntityInfoRaterUsername(host, rater);
    }
}