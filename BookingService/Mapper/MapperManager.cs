
using BookingService.Mapper.AccommodationMapper;
using BookingService.Model.Dto;
using BookingService.Model.Entity;
using BookingService.Model.Messages;

namespace BookingService.Mapper;

public class MapperManager(
    IBaseMapper<AvailabilityPeriod, AvailabilityPeriodDto> AvailabilityPeriodToAvailabilityPeriodDtoMapper,
    IBaseMapper<AccommodationCreatedDto, Accommodation> AccommodationToAccommodationCreatedDtoMapper) : IMapperManager
{
    public IBaseMapper<AvailabilityPeriod, AvailabilityPeriodDto> AvailabilityPeriodToAvailabilityPeriodDtoMapper { get; } =
        AvailabilityPeriodToAvailabilityPeriodDtoMapper;

    public IBaseMapper<AccommodationCreatedDto, Accommodation> AccommodationToAccommodationCreatedDtoMapper { get; } =
        AccommodationToAccommodationCreatedDtoMapper;

}
