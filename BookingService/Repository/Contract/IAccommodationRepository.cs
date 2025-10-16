namespace BookingService.Repository.Contract;

public interface IAccommodationRepository : ICrudRepository<Accommodation>
{    
    Task<Accommodation?> GetByExternalIdAsync(string externalId);
}