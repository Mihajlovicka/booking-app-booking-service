using BookingService.Service.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Controllers;

[Authorize(Roles = "GUEST, HOST")]
[ApiController]
[Route("api/reservations")]
public class ReservationsController(IReservationService reservationService, IUserContext userContext) : ControllerBase
{
    [HttpGet("my")]
    public async Task<IActionResult> GetMy()
    {
        var username = userContext.Name;
        return Ok(await reservationService.GetMy(username));
    }
    
    [HttpGet("{accommodationId}")]
    public async Task<IActionResult> GetAll(string accommodationId)
    {
        return Ok(await reservationService.GetByAccommodation(accommodationId));
    }
    
    [HttpDelete("{reservationId:int}")]
    public async Task<IActionResult> Cancel(int reservationId)
    {
        await reservationService.Cancel(reservationId);
        return NoContent();
    }
}