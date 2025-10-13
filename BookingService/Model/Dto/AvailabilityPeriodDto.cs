namespace BookingService.Model.Dto;

public class AvailabilityPeriodDto
{
    public int? Id { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public decimal Price { get; set; }
}
