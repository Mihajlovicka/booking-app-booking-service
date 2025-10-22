using BookingService.Service.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Controllers;

[Authorize(Roles = "GUEST, HOST")]
[ApiController]
[Route("api/accommodations/{accommodationId}/reservations")]
public class ReservationsController(IReservationService reservationService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(string accommodationId)
    {
        return Ok(await reservationService.GetByAccommodation(accommodationId));
    }
}