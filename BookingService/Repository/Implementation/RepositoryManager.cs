using BookingService.Repository.Contract;

namespace BookingService.Repository.Implementation;

public class RepositoryManager(
    IAvailabilityPeriodRepository availabilityPeriodRepository,
    IAccommodationRepository accommodationRepository) : IRepositoryManager
{
    public IAvailabilityPeriodRepository AvailabilityPeriodRepository { get; } = availabilityPeriodRepository;
    public IAccommodationRepository AccommodationRepository { get; } = accommodationRepository;

}