using BookingService.Model.Dto;
using BookingService.Service.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Controllers;

[ApiController]
[Route("api/accommodations/{accommodationId}/availability")]
public class AvailabilityController(IAvailabilityService availabilityService) : ControllerBase
{
    [Authorize(Roles = "HOST, GUEST")]
    [HttpGet]
    public async Task<IActionResult> GetAll(string accommodationId, [FromQuery] bool? fromToday = null)
    {
        return Ok(await availabilityService.GetByAccommodation(accommodationId, fromToday));
    }

    [Authorize(Roles = "HOST")]
    [HttpPost]
    public async Task<IActionResult> Add(string accommodationId, [FromBody] AvailabilityPeriodDto dto)
    {
        return Ok(await availabilityService.Add(accommodationId, dto));
    }

    [Authorize(Roles = "HOST")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await availabilityService.Delete(id);
        return NoContent();
    }
}
