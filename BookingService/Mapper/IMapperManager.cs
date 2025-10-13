using BookingService.Model.Dto;
using BookingService.Model.Entity;
using BookingService.Model.Messages;

namespace BookingService.Mapper;

public interface IMapperManager
{
    IBaseMapper<AvailabilityPeriod, AvailabilityPeriodDto> AvailabilityPeriodToAvailabilityPeriodDtoMapper { get; }
    IBaseMapper<AccommodationCreatedDto, Accommodation> AccommodationToAccommodationCreatedDtoMapper { get; }
}