using BookingService.Model.Dto;
using BookingService.Model.Entity;

namespace BookingService.Mapper.AccommodationMapper;

public class AvailabilityPeriodToAvailabilityPeriodDtoMapper(
    ) : BaseMapper<AvailabilityPeriod, AvailabilityPeriodDto>
{
    public override async Task<AvailabilityPeriodDto> Map(AvailabilityPeriod source)
    {
        return new AvailabilityPeriodDto()
        {
            Id = source.Id,
            StartDate = source.StartDate.ToString("yyyy-MM-dd"),
            EndDate = source.EndDate.ToString("yyyy-MM-dd"),
            Price = source.Price
        };
    }
}