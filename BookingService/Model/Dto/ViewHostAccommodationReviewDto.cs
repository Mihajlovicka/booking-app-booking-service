namespace BookingService.Model.Dto;

public class ViewHostAccommodationReviewDto
{
    public double HostGrade { get; set; }
    public string HostUsername { get; set; }
    public double AccommodationGrade { get; set; }
    public string AccommodationName { get; set; }
    public string AccommodationExternalId { get; set; }
    public string RaterUserName { get; set; }
}