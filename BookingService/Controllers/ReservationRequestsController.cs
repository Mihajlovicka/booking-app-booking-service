using BookingService.Model.Dto;
using BookingService.Service.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace BookingService.Controllers;

[Authorize(Roles = "GUEST, HOST")]
[ApiController]
[Route("api/reservation-requests")]
public class ReservationRequestsController(IReservationRequestService reservationRequestService, IUserContext userContext) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateReservationRequestDto dto)
    {
        var username = userContext.Name;
        return Ok(await reservationRequestService.Add(username, dto));
    }
    
    [HttpGet]
    public async Task<IActionResult> GetMyReservationRequests()
    {
        var username = userContext.Name;
        return Ok(await reservationRequestService.GetMyReservationRequests(username));
    }

    [HttpGet("{accommodationId}")]
    public async Task<IActionResult> GetAllForAccommodation(string accommodationId)
    {
        return Ok(await reservationRequestService.GetAllForAccommodation(accommodationId));
    }

    [HttpDelete("{reservationRequestId}")]
    public async Task<IActionResult> RejectRequest(string reservationRequestId)
    {
        await reservationRequestService.RejectRequest(reservationRequestId);
        return NoContent();
    }
    
    [HttpPost("{reservationRequestId}")]
    public async Task<IActionResult> AcceptRequest(string reservationRequestId)
    {
        await reservationRequestService.AcceptRequest(reservationRequestId);
        return NoContent();
    }
}