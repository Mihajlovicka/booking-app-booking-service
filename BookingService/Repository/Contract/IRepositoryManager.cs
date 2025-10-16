namespace BookingService.Repository.Contract;

public interface IRepositoryManager
{
   IAvailabilityPeriodRepository AvailabilityPeriodRepository { get; }
   IAccommodationRepository AccommodationRepository { get; }
   IReservationRepository ReservationRepository { get; }
   IReservationRequestRepository ReservationRequestRepository { get; }
   IUserRepository UserRepository { get; }
}