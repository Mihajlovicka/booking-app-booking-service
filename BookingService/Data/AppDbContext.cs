using BookingService.Model.Entity;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options)
{

    public AppDbContext() : this(new DbContextOptions<AppDbContext>())
    {
    }
    
    public DbSet<Accommodation> Accommodations { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<AvailabilityPeriod> AvailabilityPeriods { get; set; }
    public DbSet<ReservationRequest> ReservationRequests { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<User> Users { get; set; }
}
