using BookingService.Filters;
using BookingService.Mapper;
using BookingService.Mapper.AccommodationMapper;
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

        // Mapper-related scoped services
        services.AddScoped<IMapperManager, MapperManager>();
        services.AddScoped<IBaseMapper<AccommodationCreatedDto, Accommodation>, AccommodationCreatedDtoToAccommodationMapper>();
        services.AddScoped<IBaseMapper<AvailabilityPeriod, AvailabilityPeriodDto>, AvailabilityPeriodToAvailabilityPeriodDtoMapper>();

        // Repository-related scoped services
        services.AddScoped<IRepositoryManager, RepositoryManager>();
        services.AddScoped<IAccommodationRepository, AccommodationRepository>();
        services.AddScoped<IAvailabilityPeriodRepository, AvailabilityPeriodRepository>();


        return services;
    }
}
