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

    public async Task<int> DeleteByUsernameAsync(string username)
    {
        var users = await _dbSet
            .Where(a => a.Username == username)
            .ToListAsync();

        if (users.Count == 0)
            return 0;

        _dbSet.RemoveRange(users);
        return await _context.SaveChangesAsync();
    }
}