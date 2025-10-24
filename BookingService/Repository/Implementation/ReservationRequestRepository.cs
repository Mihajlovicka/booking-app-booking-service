using BookingService.Data;
using BookingService.Model.Entity;
using BookingService.Repository.Contract;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Repository.Implementation;

public class ReservationRequestRepository(AppDbContext context)
    : CrudRepository<ReservationRequest>(context), IReservationRequestRepository
{
    public async Task<IEnumerable<ReservationRequest>> GetAllForAccommodation(string accommodationId)
    {
        return await _context.ReservationRequests
            .Include(r => r.Guest)
            .Include(r => r.Accommodation)
            .Where(r => r.Accommodation.ExternalId == accommodationId)
            .ToListAsync();
    }

    public async Task<ReservationRequest?> GerByExternalId(Guid externalId)
    {
        return await _context.ReservationRequests
            .Include(r => r.Guest)
            .Include(r => r.Accommodation)
            .FirstOrDefaultAsync(r => r.ExternalId == externalId);
    }
    
    public async Task<IEnumerable<ReservationRequest>> Overlaps(string accommodationId, DateTime start, DateTime end)
    {
        return await _dbSet
            .Include(p=> p.Guest)
            .Include(p => p.Accommodation)
            .Where(p => 
                p.Accommodation.ExternalId == accommodationId &&
                p.StartDate < end && p.EndDate > start
                ).ToListAsync();
    }

    public async Task<IEnumerable<ReservationRequest>> GetMy(string username)
    {
        return await _context.ReservationRequests
            .Include(r => r.Guest)
            .Include(r => r.Accommodation)
            .Where(r => r.Guest.Username == username).ToListAsync();

    }
    public async Task<int> DeleteByGuestAsync(int UserId)
    {
        var users = await _dbSet
            .Where(a => a.UserId == UserId)
            .ToListAsync();

        if (users.Count == 0)
            return 0;

        _dbSet.RemoveRange(users);
        return await _context.SaveChangesAsync();

    }
}