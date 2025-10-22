using BookingService.Model.Dto;
using BookingService.Service.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace BookingService.Controllers;

[Authorize(Roles = "GUEST")]
[ApiController]
[Route("api/accommodations/{accommodationId}/reservation-requests")]
public class ReservationRequestsController(IReservationRequestService reservationRequestService, IUserContext userContext) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateReservationRequestDto dto)
    {
        var username = userContext.Name;
        return Ok(await reservationRequestService.Add(username, dto));
    }
}