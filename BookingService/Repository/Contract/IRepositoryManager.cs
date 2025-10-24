using BookingService.Repository.Contract;

namespace BookingService.Repository.Contract;

public interface IRepositoryManager
{
   IAvailabilityPeriodRepository AvailabilityPeriodRepository { get; }
   IAccommodationRepository AccommodationRepository { get; }
   IAddressRepository AddressRepository { get; }
   IReservationRepository ReservationRepository { get; }
   IReservationRequestRepository ReservationRequestRepository { get; }
   IUserRepository UserRepository { get; }
   IReviewRepository ReviewRepository { get; }
}