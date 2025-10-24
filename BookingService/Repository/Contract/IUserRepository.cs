using BookingService.Model.Entity;

namespace BookingService.Repository.Contract;

public interface IUserRepository : ICrudRepository<User>
{
    Task<User?> GetByUsernameAsync(string username);
    Task<int> DeleteByUsernameAsync(string username);
}