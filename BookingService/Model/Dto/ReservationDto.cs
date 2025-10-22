namespace BookingService.Model.Dto;

public class ReservationDto
{
    public int Id { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public string AccommodationExternalId { get; set; }
    public string GuestUsername { get; set; }
    public int GuestNumber { get; set; }
    public decimal FinalPrice { get; set; }
}