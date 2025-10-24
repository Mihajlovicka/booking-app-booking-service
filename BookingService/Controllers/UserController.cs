using BookingService.Service.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Controllers;

[Authorize(Roles = "GUEST, HOST")]
[ApiController]
[Route("api/user")]
public class UserController(IAvailabilityService availabilityService) : ControllerBase
{
    [HttpGet("delete-check")]
    public async Task<IActionResult> DeleteCheckStatus()
    {
        return Ok(await availabilityService.UserDeleteCheckStatus());
    }
}