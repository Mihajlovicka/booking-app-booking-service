namespace BookingService.Model.Dto;

public class ReservationRequestDto
{
    public string GuestUsername { get; set; }
    public int GuestNumber { get; set; }
    public decimal FinalPrice { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public Guid ExternalId { get; set; }
    public string AccommodationExternalId { get; set; }
    public string? AccommodationName { get; set; }
    public int UserCancellationNumber { get; set; }
}