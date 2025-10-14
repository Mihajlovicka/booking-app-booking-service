namespace BookingService.Model.Dto;

public class AvailabilityFilterDto
{
    public string? StartDate { get; set; }
    public string? EndDate { get; set; }
    public string? Address { get; set; }
    public int? NumberOfGuests { get; set; }
}
