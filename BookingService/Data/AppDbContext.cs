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
    
}
