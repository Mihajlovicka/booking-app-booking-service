using BookingService.Model.Dto;

namespace BookingService.Service.Contract;

public interface IReservationRequestService
{
    Task<ReservationRequestDto> Add(string guestUsername, CreateReservationRequestDto dto);
}