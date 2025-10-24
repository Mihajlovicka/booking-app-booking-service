using BookingService.Model.Dto;
using BookingService.Service.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Controllers;

[Authorize(Roles = "GUEST, HOST")]
[ApiController]
[Route("api/reviews")]
public class ReviewController(IReviewService reviewService, IUserContext userContext) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] IEnumerable<ReviewDto> dtos)
    {
        var username = userContext.Name ?? "";

        foreach (var dto in dtos)
        {
            dto.RaterUsername = username;
        }

        await reviewService.Add(dtos);

        return NoContent();
    }
}