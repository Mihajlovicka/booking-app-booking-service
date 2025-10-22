using BookingService.Model.Dto;
using BookingService.Model.Entity;

namespace BookingService.Mapper.ReservationMapper;

public class ReservationToReservationDtoMapper : BaseMapper<Reservation, ReservationDto>
{
    public override Task<ReservationDto> Map(Reservation source)
    {
        var dto = new ReservationDto
        {
            Id = source.Id,
            AccommodationExternalId = source.Accommodation.ExternalId,
            GuestNumber = source.GuestNumber,
            GuestUsername = source.GuestUsername,
            FinalPrice = source.FinalPrice,
            StartDate = source.StartDate.ToString("yyyy-MM-dd"),
            EndDate = source.EndDate.ToString("yyyy-MM-dd"),
        };

        return Task.FromResult(dto);
    }
}