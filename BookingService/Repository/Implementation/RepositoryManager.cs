using BookingService.Repository.Contract;

namespace BookingService.Repository.Implementation;

public class RepositoryManager(
    IAvailabilityPeriodRepository availabilityPeriodRepository,
    IAccommodationRepository accommodationRepository,
    IAddressRepository addressRepository,
    IReservationRepository reservationRepository,
    IReservationRequestRepository reservationRequestRepository,
    IUserRepository userRepository,
    IReviewRepository reviewRepository) : IRepositoryManager
{
    public IAvailabilityPeriodRepository AvailabilityPeriodRepository { get; } = availabilityPeriodRepository;
    public IAccommodationRepository AccommodationRepository { get; } = accommodationRepository;
    public IAddressRepository AddressRepository { get; } = addressRepository;
    public IReservationRepository ReservationRepository { get; } = reservationRepository;
    public IReservationRequestRepository ReservationRequestRepository { get; } = reservationRequestRepository;
    public IUserRepository UserRepository { get; } = userRepository;
    public IReviewRepository ReviewRepository { get; } = reviewRepository;
}