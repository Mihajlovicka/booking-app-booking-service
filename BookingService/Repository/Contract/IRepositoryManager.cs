namespace BookingService.Repository.Contract;

public interface IRepositoryManager
{
   IAvailabilityPeriodRepository AvailabilityPeriodRepository { get; }
   IAccommodationRepository AccommodationRepository { get; }
}