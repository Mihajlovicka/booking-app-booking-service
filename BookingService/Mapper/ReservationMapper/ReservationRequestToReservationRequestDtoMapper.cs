using BookingService.Model.Dto;
using BookingService.Model.Entity;

namespace BookingService.Mapper.ReservationMapper;

public class ReservationRequestToReservationRequestDtoMapper : BaseMapper<ReservationRequest, ReservationRequestDto>
{
    public override Task<ReservationRequestDto> Map(ReservationRequest source)
    {
        var dto = new ReservationRequestDto
        {
            GuestUsername = source.Guest.Username,
            GuestNumber = source.GuestNumber,
            FinalPrice = source.FinalPrice,
            StartDate = source.StartDate.ToString("yyyy-MM-dd"),
            EndDate = source.EndDate.ToString("yyyy-MM-dd"),
            ExternalId = source.ExternalId,
            AccommodationExternalId = source.Accommodation.ExternalId,
            AccommodationName = source.Accommodation.Name
        };

        return Task.FromResult(dto);
    }
}