using BookingService.Model.Dto;

namespace BookingService.Service.Contract;

public interface IReservationRequestService
{
    Task<ReservationRequestDto> Add(string guestUsername, CreateReservationRequestDto dto);
    Task<IEnumerable<ReservationRequestDto>> GetAllForAccommodation(string accommodationId);
    Task RejectRequest(string requestExternalId);
    Task AcceptRequest(string requestExternalId);
    Task<IEnumerable<ReservationRequestDto>> GetMyReservationRequests(string username);
}