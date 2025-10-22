using BookingService.Model.Dto;
using BookingService.Model.Entity;

namespace BookingService.Service.Contract;

public interface IReservationService
{
    Task<IEnumerable<ReservationDto>> GetByAccommodation(string accommodationId);
    Task CreateReservation(ReservationRequest request);
} 