using BookingService.Model.Dto;
using BookingService.Model.Entity;
using BookingService.Model.Messages;

namespace BookingService.Mapper;

public class MapperManager(
    IBaseMapper<AvailabilityPeriod, AvailabilityPeriodDto> AvailabilityPeriodToAvailabilityPeriodDtoMapper,
    IBaseMapper<AccommodationCreatedDto, Accommodation> AccommodationToAccommodationCreatedDtoMapper,
    IBaseMapper<UserDto, User> UserDtoToUserMapper,
    IBaseMapper<CreateReservationRequestDto, ReservationRequest> CreateReservationRequestDtoToReservationRequestMapper,
    IBaseMapper<ReservationRequest, ReservationRequestDto> ReservationRequestToReservationRequestDtoMapper,
    IBaseMapper<Reservation, ReservationDto> ReservationToReservationDtoMapper) : IMapperManager
{
    public IBaseMapper<AvailabilityPeriod, AvailabilityPeriodDto> AvailabilityPeriodToAvailabilityPeriodDtoMapper { get; } =
        AvailabilityPeriodToAvailabilityPeriodDtoMapper;

    public IBaseMapper<AccommodationCreatedDto, Accommodation> AccommodationToAccommodationCreatedDtoMapper { get; } =
        AccommodationToAccommodationCreatedDtoMapper;

    public IBaseMapper<UserDto, User> UserDtoToUserMapper { get; } = UserDtoToUserMapper;

    public IBaseMapper<CreateReservationRequestDto, ReservationRequest>
        CreateReservationRequestDtoToReservationRequestMapper
    {
        get;
    } = CreateReservationRequestDtoToReservationRequestMapper;

    public IBaseMapper<ReservationRequest, ReservationRequestDto> ReservationRequestToReservationRequestDtoMapper
    {
        get;
    } = ReservationRequestToReservationRequestDtoMapper;

    public IBaseMapper<Reservation, ReservationDto> ReservationToReservationDtoMapper { get; } =
        ReservationToReservationDtoMapper;
}
