using System.Threading.Tasks;
using BookingService.Mapper;
using BookingService.Model.Dto;
using BookingService.Service.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Controllers;

[Authorize(Roles = "HOST")]
[ApiController]
[Route("api/accommodations/{accommodationId}/availability")]
public class AvailabilityController(IAvailabilityService availabilityService) : ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> GetAll(string accommodationId)
    {
        return Ok(await availabilityService.GetByAccommodation(accommodationId));
    }

    [HttpPost]
    public async Task<IActionResult> Add(string accommodationId, [FromBody] AvailabilityPeriodDto dto)
    {
        return Ok(await availabilityService.Add(accommodationId, dto));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await availabilityService.Delete(id);
        return NoContent();
    }
}
