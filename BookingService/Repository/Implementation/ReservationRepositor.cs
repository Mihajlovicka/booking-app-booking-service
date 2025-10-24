using BookingService.Data;
using BookingService.Model.Entity;
using BookingService.Repository.Contract;
using BookingService.Service.Contract;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Repository.Implementation;

public class ReservationRepository(AppDbContext context, IUserContext userContext) : CrudRepository<Reservation>(context), IReservationRepository
{
    public async Task<List<Reservation>> GetAllReservationsForAccommodation(string accommodationId)
    {
        return await _dbSet
            .Include(p => p.Accommodation)
            .Where(p => p.Accommodation.ExternalId == accommodationId && p.Active)
            .ToListAsync();
    }
    public bool Overlaps(string accommodationId, DateTime start, DateTime end)
    {
        return _dbSet
            .Include(p => p.Accommodation)
            .Any(p =>
                p.Accommodation.ExternalId == accommodationId &&
                p.StartDate < end && p.EndDate > start &&
                p.Active
            );
    }

    public async Task<List<Reservation>> GetMy(string username)
    {
        return await _dbSet
            .Include(p => p.Accommodation)
            .Where(p => p.GuestUsername == username && p.Active)
            .ToListAsync();
    }

    public async Task<int> GetUserCancellationNumber(string username)
    {
        return (await _dbSet
            .Include(p => p.Accommodation)
            .Where(p => p.GuestUsername == username && !p.Active)
            .ToListAsync()).Count;
    }

    public async Task<Reservation> GetByIdAsyncWithAccomodation(int reservationId)
    {
        return await _dbSet
            .Include(p => p.Accommodation)
            .Where(p => p.Id == reservationId)
            .FirstOrDefaultAsync();
    }

    public async Task<int> GetFutureReservationCountAsync()
    {
        var today = DateTime.Now.Date;
        var role = userContext.Role;
        var username = userContext.Name;

        IQueryable<Reservation> query = _dbSet
            .Include(r => r.Accommodation)
            .Where(r => r.EndDate >= today && r.Active);

        if (role == Role.HOST.ToString())
        {
            query = query.Where(r => r.Accommodation.Owner == username);
        }
        else if (role == Role.GUEST.ToString())
        {
            query = query.Where(r => r.GuestUsername == username);
        }

        return await query.CountAsync();
    }

        public async Task<int> DeleteByGuestAsync(string username)
    {
        var users = await _dbSet
            .Where(a => a.GuestUsername == username)
            .ToListAsync();

        if (users.Count == 0)
            return 0;

        _dbSet.RemoveRange(users);
        return await _context.SaveChangesAsync();
    }
}