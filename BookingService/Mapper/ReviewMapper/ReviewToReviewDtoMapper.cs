using BookingService.Model.Dto;
using BookingService.Model.Entity;

namespace BookingService.Mapper.ReviewMapper;

public class ReviewToReviewDtoMapper : BaseMapper<Review, ReviewDto>
{
    public override async Task<ReviewDto> Map(Review source)
    {
        var res = new ReviewDto()
        {
            Id = source.Id,
            RaterUsername = source.RaterUsername,
            Grade = source.Grade,
            ReviewFor = source.ReviewFor.ToString(),
            EntityInfo = source.EntityInfo
        };

        return res;
    }
}