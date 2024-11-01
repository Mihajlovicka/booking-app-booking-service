using Microsoft.EntityFrameworkCore;

namespace BookingService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
}
