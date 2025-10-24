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
    public DbSet<ReservationRequest?> ReservationRequests { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Review> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 🏠 Accommodation → AvailabilityPeriods (one-to-many, cascade)
        modelBuilder.Entity<AvailabilityPeriod>()
            .HasOne(p => p.Accommodation)
            .WithMany(a => a.AvailabilityPeriods)
            .HasForeignKey(p => p.AccommodationId)
            .OnDelete(DeleteBehavior.Cascade);

        // 🏠 Accommodation → Pictures (one-to-many, cascade)
        modelBuilder.Entity<Picture>()
            .HasOne(p => p.Accommodation)
            .WithMany(a => a.Pictures)
            .HasForeignKey(p => p.AccommodationId)
            .OnDelete(DeleteBehavior.Cascade);


        // 🏠 Accommodation → Address
        modelBuilder.Entity<Accommodation>()
            .HasOne(a => a.Address)
            .WithMany()
            .HasForeignKey(a => a.AddressId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
