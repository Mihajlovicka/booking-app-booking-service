using System.Threading.Tasks;
using BookingService.Mapper;
using BookingService.Model.Dto;
using BookingService.Service.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Controllers;

[ApiController]
[Route("api/accommodations")]
public class SearchController(IAvailabilityService availabilityService) : ControllerBase
{
    [HttpPost("search")]
    public async Task<IActionResult> SearchAvailability([FromBody] AvailabilityFilterDto? availabilityFilterDto)
    {
        return Ok(await availabilityService.Search(availabilityFilterDto));
    }
}