using System.Globalization;
using BookingService.Model.Dto;
using BookingService.Model.Entity;

namespace BookingService.Mapper.ReservationMapper;

public class CreateReservationRequestDtoToReservationRequestMapper()
    : BaseMapper<CreateReservationRequestDto, ReservationRequest>
{
    public override Task<ReservationRequest> Map(CreateReservationRequestDto source)
    {
        var reservationRequest = new ReservationRequest();

        reservationRequest.GuestNumber = source.GuestNumber;
        reservationRequest.ExternalId = Guid.NewGuid();
        
        if (DateTime.TryParseExact(
                source.StartDate, 
                "yyyy-MM-dd", 
                CultureInfo.InvariantCulture, 
                DateTimeStyles.None, 
                out var startDate))
        {
            reservationRequest.StartDate = startDate;
        }
        else
        {
            throw new ArgumentException($"Invalid StartDate format: {source.StartDate}");
        }
        
        if (DateTime.TryParseExact(
                source.EndDate, 
                "yyyy-MM-dd", 
                CultureInfo.InvariantCulture, 
                DateTimeStyles.None, 
                out var endDate))
        {
            reservationRequest.EndDate = endDate;
        }
        else
        {
            throw new ArgumentException($"Invalid EndDate format: {source.EndDate}");
        }

        return Task.FromResult(reservationRequest);
    }
}