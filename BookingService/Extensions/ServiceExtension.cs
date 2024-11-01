using BookingService.Filters;
using BookingService.Mapper;
using BookingService.Repository.Contract;
using BookingService.Repository.Implementation;

namespace BookingService.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        // Scoped services registration
        services.AddScoped<ValidationFilterAttribute>();

        // Mapper-related scoped services
        services.AddScoped<IMapperManager, MapperManager>();

        // Repository-related scoped services
        services.AddScoped<IRepositoryManager, RepositoryManager>();

        return services;
    }
}
