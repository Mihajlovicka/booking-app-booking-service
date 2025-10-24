using System.Linq.Expressions;
using BookingService.Model.Dto;
using BookingService.Model.Entity;

namespace BookingService.Repository.Contract;

public interface IAccommodationRepository : ICrudRepository<Accommodation>
{
    Task<Accommodation?> GetByExternalIdAsync(string externalId);
    Task<IEnumerable<Accommodation>> Search(AvailabilityFilterDto availabilityFilterDto);
    Task<int> DeleteByOwnerAsync(string ownerUsername);
}