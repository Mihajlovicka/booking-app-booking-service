using BookingService.Model.Dto;
using BookingService.Model.Entity;

namespace BookingService.Service.Contract;

public interface IReservationService
{
    Task<IEnumerable<ReservationDto>> GetByAccommodation(string accommodationId);
    Task CreateReservation(ReservationRequest request);
    Task<IEnumerable<ReservationDto>> GetMy(string username);
    Task Cancel(int reservationId);
} 