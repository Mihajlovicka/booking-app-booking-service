using BookingService.Repository.Contract;

namespace BookingService.Repository.Implementation;

public class RepositoryManager(
    IAvailabilityPeriodRepository availabilityPeriodRepository,
    IAccommodationRepository accommodationRepository,
    IReservationRepository reservationRepository,
    IReservationRequestRepository reservationRequestRepository,
    IUserRepository userRepository) : IRepositoryManager
{
    public IAvailabilityPeriodRepository AvailabilityPeriodRepository { get; } = availabilityPeriodRepository;
    public IAccommodationRepository AccommodationRepository { get; } = accommodationRepository;
    public IReservationRepository ReservationRepository { get; } = reservationRepository;
    public IReservationRequestRepository ReservationRequestRepository { get; } = reservationRequestRepository;
    public IUserRepository UserRepository { get; } = userRepository;
}