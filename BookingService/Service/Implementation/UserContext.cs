using System.Security.Claims;
using BookingService.Service.Contract;

namespace BookingService.Service.Implementation;

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public string? Name => httpContextAccessor.HttpContext?.User?.Claims
        .FirstOrDefault(c => c.Type == "name")?.Value ?? httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

    public string? Role => httpContextAccessor.HttpContext?.User?.Claims
        .FirstOrDefault(c => c.Type == "role")?.Value ?? httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;
}
