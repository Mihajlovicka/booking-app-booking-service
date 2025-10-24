using BookingService.Model.Dto;
using BookingService.Model.Entity;
using BookingService.Model.Messages;

namespace BookingService.Mapper;

public interface IMapperManager
{
    IBaseMapper<AvailabilityPeriod, AvailabilityPeriodDto> AvailabilityPeriodToAvailabilityPeriodDtoMapper { get; }
    IBaseMapper<AccommodationCreatedDto, Accommodation> AccommodationToAccommodationCreatedDtoMapper { get; }
    IBaseMapper<AddressDto, Address> AddressDtoToAddressMapper { get; }
    IBaseMapper<Accommodation, AccommodationDto> AccommodationToAccommodationDtoMapper { get; }
    IBaseMapper<UserDto, User> UserDtoToUserMapper { get; }
    IBaseMapper<CreateReservationRequestDto, ReservationRequest> CreateReservationRequestDtoToReservationRequestMapper
    {
        get;
    }
    IBaseMapper<ReservationRequest, ReservationRequestDto> ReservationRequestToReservationRequestDtoMapper { get; }
    IBaseMapper<Reservation, ReservationDto> ReservationToReservationDtoMapper { get; }
    IBaseMapper<Review, ReviewDto> ReviewToReviewDtoMapper { get; }
}