using BookingService.Model.Dto;
using BookingService.Model.Entity;
using BookingService.Model.Messages;

namespace BookingService.Mapper;

public class MapperManager(
    IBaseMapper<AvailabilityPeriod, AvailabilityPeriodDto> AvailabilityPeriodToAvailabilityPeriodDtoMapper,
    IBaseMapper<AccommodationCreatedDto, Accommodation> AccommodationToAccommodationCreatedDtoMapper,
    IBaseMapper<AddressDto, Address> AddressDtoToAddressMapper,
    IBaseMapper<Accommodation, AccommodationDto> AccommodationToAccommodationDtoMapper,
    IBaseMapper<UserDto, User> UserDtoToUserMapper,
    IBaseMapper<CreateReservationRequestDto, ReservationRequest> CreateReservationRequestDtoToReservationRequestMapper,
    IBaseMapper<ReservationRequest, ReservationRequestDto> ReservationRequestToReservationRequestDtoMapper,
    IBaseMapper<Reservation, ReservationDto> ReservationToReservationDtoMapper) : IMapperManager
{
    public IBaseMapper<AvailabilityPeriod, AvailabilityPeriodDto> AvailabilityPeriodToAvailabilityPeriodDtoMapper { get; } =
        AvailabilityPeriodToAvailabilityPeriodDtoMapper;

    public IBaseMapper<AccommodationCreatedDto, Accommodation> AccommodationToAccommodationCreatedDtoMapper { get; } =
        AccommodationToAccommodationCreatedDtoMapper;

    public IBaseMapper<AddressDto, Address> AddressDtoToAddressMapper { get; } = AddressDtoToAddressMapper;

    public IBaseMapper<Accommodation, AccommodationDto> AccommodationToAccommodationDtoMapper { get; } = AccommodationToAccommodationDtoMapper;

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
