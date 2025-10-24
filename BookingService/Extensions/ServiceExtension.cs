using BookingService.Filters;
using BookingService.Mapper;
using BookingService.Mapper.AccommodationMapper;
using BookingService.Mapper.AvailabilityMapper;
using BookingService.Mapper.ReservationMapper;
using BookingService.Mapper.ReviewMapper;
using BookingService.Mapper.UserMapper;
using BookingService.Model.Dto;
using BookingService.Model.Entity;
using BookingService.Model.Messages;
using BookingService.Repository.Contract;
using BookingService.Repository.Implementation;
using BookingService.Service.Contract;
using BookingService.Service.Implementation;

namespace BookingService.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContext>();
        // Scoped services registration
        services.AddScoped<ValidationFilterAttribute>();

        services.AddScoped<IAvailabilityService, AvailabilityService>();
        services.AddScoped<IReservationRequestService, ReservationRequestService>();
        services.AddScoped<IReservationService, ReservationService>();
        services.AddScoped<IReviewService, ReviewService>();

        // Mapper-related scoped services
        services.AddScoped<IMapperManager, MapperManager>();
        services.AddScoped<IBaseMapper<AccommodationCreatedDto, Accommodation>, AccommodationCreatedDtoToAccommodationMapper>();
        services.AddScoped<IBaseMapper<AvailabilityPeriod, AvailabilityPeriodDto>, AvailabilityPeriodToAvailabilityPeriodDtoMapper>();
        services.AddScoped<IBaseMapper<AddressDto, Address>, AddressDtoToAddressMapper>();
        services.AddScoped<IBaseMapper<Accommodation, AccommodationDto>, AccommodationToAccommodationDtoMapper>();
        services.AddScoped<IBaseMapper<UserDto, User>, UserDtoToUserMapper>();
        services.AddScoped<IBaseMapper<CreateReservationRequestDto, ReservationRequest>, CreateReservationRequestDtoToReservationRequestMapper>();
        services.AddScoped<IBaseMapper<ReservationRequest, ReservationRequestDto>, ReservationRequestToReservationRequestDtoMapper>();
        services.AddScoped<IBaseMapper<Reservation, ReservationDto>, ReservationToReservationDtoMapper>();
        services.AddScoped<IBaseMapper<Review, ReviewDto>, ReviewToReviewDtoMapper>();

        // Repository-related scoped services
        services.AddScoped<IRepositoryManager, RepositoryManager>();
        services.AddScoped<IAccommodationRepository, AccommodationRepository>();
        services.AddScoped<IAvailabilityPeriodRepository, AvailabilityPeriodRepository>();
        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<IReservationRepository, ReservationRepository>();
        services.AddScoped<IReservationRequestRepository, ReservationRequestRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();

        return services;
    }
}
