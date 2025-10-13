using BookingService.Model.Entity;

namespace BookingService.Model.Messages;

public class AccommodationCreatedDto
{
    public string Id { get; set; }
    public PriceType PriceType { get; set; }
    public string Owner { get; set; }
}