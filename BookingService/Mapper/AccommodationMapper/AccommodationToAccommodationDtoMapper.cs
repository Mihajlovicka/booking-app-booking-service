using BookingService.Model.Dto;
using BookingService.Model.Entity;
using BookingService.Repository.Contract;

namespace BookingService.Mapper.AccommodationMapper;

public class AccommodationToAccommodationDtoMapper(
    IBaseMapper<AddressDto, Address> addressDtoToAddressMapper,
    IRepositoryManager repositoryManager
    ) : BaseMapper<Accommodation, AccommodationDto>
{
    public override async Task<AccommodationDto> Map(Accommodation source)
    {
        var grades = (await repositoryManager.ReviewRepository
                .GetByEntityInfo(source.ExternalId.ToString()))
            .Select(x => x.Grade);

        var enumerable = grades as int[] ?? grades.ToArray();
        var averageGrade = enumerable.Length == 0 ? 0 : enumerable.Average();
        
        return new AccommodationDto()
        {
            Name = source.Name,
            MinNumberOfGuests = source.MinNumberOfGuests,
            MaxNumberOfGuests = source.MaxNumberOfGuests,
            Id = source.ExternalId.ToString(),
            PriceType = source.PriceType.ToString(),
            Address = addressDtoToAddressMapper.ReverseMap(source.Address),
            Pictures = source.Pictures.Select(picture => picture.Url).ToList(),
            AverageGrade = averageGrade
        };
    }
}