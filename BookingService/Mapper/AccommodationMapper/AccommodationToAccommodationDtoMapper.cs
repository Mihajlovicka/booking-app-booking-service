using BookingService.Model.Dto;
using BookingService.Model.Entity;

namespace BookingService.Mapper.AccommodationMapper;

public class AccommodationToAccommodationDtoMapper(
    IBaseMapper<AddressDto, Address> addressDtoToAddressMapper
    ) : BaseMapper<Accommodation, AccommodationDto>
{
    public override async Task<AccommodationDto> Map(Accommodation source)
    {
        return new AccommodationDto()
        {
            Name = source.Name,
            MinNumberOfGuests = source.MinNumberOfGuests,
            MaxNumberOfGuests = source.MaxNumberOfGuests,
            Id = source.ExternalId.ToString(),
            PriceType = source.PriceType.ToString(),
            Address = addressDtoToAddressMapper.ReverseMap(source.Address),
            Pictures = source.Pictures.Select(picture => picture.Url).ToList(),
        };
    }
}