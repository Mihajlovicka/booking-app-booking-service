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
    
    [HttpGet("{accommodationExternalId}/rater")]
    public async Task<IActionResult> GetAccRater(string accommodationExternalId)
    {
        var username = userContext.Name ?? "";
        return Ok(await reviewService.GetAccommodationViewGradesForRater(accommodationExternalId, username));
    }
    
    [HttpGet("{accommodationExternalId}")]
    public async Task<IActionResult> GetAccommodationAverageGrade(string accommodationExternalId)
    {
        return Ok(await reviewService.GetAverageGradeForAccommodation(accommodationExternalId));
    }
    
    [HttpDelete("{accommodationExternalId}")]
    public async Task<IActionResult> DeleteReviewForAccommodation(string accommodationExternalId)
    {
        var username = userContext.Name ?? "";
        await reviewService.DeleteReviewsForAccommodation(accommodationExternalId, username);

        return NoContent();
    }

    [HttpGet("{accommodationExternalId}/all")]
    public async Task<IActionResult> GetAccommodationViewGrades(string accommodationExternalId)
    {
        return Ok(await reviewService.GetAccommodationViewGrades(accommodationExternalId));
    }
}