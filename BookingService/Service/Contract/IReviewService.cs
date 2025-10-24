using BookingService.Model.Dto;

namespace BookingService.Service.Contract;

public interface IReviewService
{
    Task Add(IEnumerable<ReviewDto> reviewDtos);
    Task<double> GetAverageGradeForAccommodation(string externalId);
    Task<IEnumerable<ViewHostAccommodationReviewDto>> GetAccommodationViewGrades(string externalId);
    Task DeleteReviewsForAccommodation(string externalId, string raterUsername);
    Task<ViewHostAccommodationReviewDto> GetAccommodationViewGradesForRater(string externalId, string raterUsername);
}