using BookingService.Model.Entity;

namespace BookingService.Model.Dto;

public class ReviewDto
{
    public int? Id { get; set; }
    public int Grade { get; set; }
    public string? RaterUsername { get; set; }
    public string ReviewFor { get; set; }
    public string EntityInfo { get; set; }
}