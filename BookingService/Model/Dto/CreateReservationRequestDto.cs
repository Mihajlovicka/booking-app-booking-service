namespace BookingService.Model.Dto;

public class CreateReservationRequestDto
{
    public int GuestNumber { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public string AccommodationExternalId { get; set; }
}