using BookingService.Data;
using BookingService.Model.Entity;
using BookingService.Repository.Contract;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Repository.Implementation;

public class UserRepository(AppDbContext context) : CrudRepository<User>(context), IUserRepository
{
    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Username.Equals(username));
    }
}