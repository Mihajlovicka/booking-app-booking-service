using BookingService.Model.Dto;

namespace BookingService.Service.Contract;

public interface IReviewService
{
    Task Add(IEnumerable<ReviewDto> reviewDtos);
}