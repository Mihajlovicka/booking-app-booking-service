using BookingService.Model.Entity;

namespace BookingService.Repository.Contract;

public interface IAddressRepository
{
    Task<Address?> GetByIdAsync(int id);
    Task<Address?> GetByProperties(Address address);
}