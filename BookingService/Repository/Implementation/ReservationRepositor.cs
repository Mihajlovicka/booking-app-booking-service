using BookingService.Data;
using BookingService.Model.Entity;
using BookingService.Repository.Contract;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Repository.Implementation;

public class ReservationRepository(AppDbContext context) : CrudRepository<Reservation>(context), IReservationRepository
{
    public async Task<List<Reservation>> GetAllReservationsForAccommodation(string accommodationId)
    {
        return await _dbSet
            .Include(p => p.Accommodation)
            .Where(p => p.Accommodation.ExternalId == accommodationId)
            .ToListAsync();
    }
}