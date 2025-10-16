using BookingService.Model.Entity;
using BookingService.Model.Messages;

namespace BookingService.Mapper.AccommodationMapper;

public class AccommodationCreatedDtoToAccommodationMapper(
    ) : BaseMapper<AccommodationCreatedDto, Accommodation>
{
    public override async Task<Accommodation> Map(AccommodationCreatedDto source)
    {
        return new Accommodation()
        {
            ExternalId = source.Id.ToString(),
            Name = source.Name,
            PriceType = source.PriceType,
            Owner = source.Owner,
            MaxNumberOfGuests = source.MaxNumberOfGuests,
            MinNumberOfGuests = source.MinNumberOfGuests,
            Pictures = source.Pictures.Select(url => new Picture { Url = url }).ToList()
        };
    }
}