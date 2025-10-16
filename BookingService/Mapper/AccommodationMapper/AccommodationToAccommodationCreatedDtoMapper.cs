using BookingService.Model.Messages;

namespace BookingService.Mapper.AccommodationMapper;

public class AccommodationCreatedDtoToAccommodationMapper(
    ) : BaseMapper<AccommodationCreatedDto, Accommodation>
{
    public override Accommodation Map(AccommodationCreatedDto source)
    {
        return new Accommodation()
        {
            ExternalId = source.Id,
            PriceType = source.PriceType,
            Owner = source.Owner,
            MinNumberOfGuests = source.MinNumberOfGuests,
            MaxNumberOfGuests = source.MaxNumberOfGuests
        };
    }
}