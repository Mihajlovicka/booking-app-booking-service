using BookingService.Service.Contract;

namespace BookingService.Service.Implementation;

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public string? Name => httpContextAccessor.HttpContext?.Items["name"]?.ToString();
}