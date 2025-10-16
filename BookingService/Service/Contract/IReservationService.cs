using BookingService.Model.Dto;

namespace BookingService.Service.Contract;

public interface IReservationService
{
    Task<IEnumerable<ReservationDto>> GetByAccommodation(string accommodationId);
}