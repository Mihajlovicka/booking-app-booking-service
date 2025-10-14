using System.Linq.Expressions;
using BookingService.Data;
using BookingService.Model.Dto;
using BookingService.Repository.Contract;
using BookingService.Service.Contract;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Repository.Implementation;

public class AccommodationRepository(AppDbContext context, IUserContext userContext)
    : CrudRepository<Accommodation>(context), IAccommodationRepository
{
    public async Task<Accommodation?> GetByExternalIdAsync(string externalId)
    {
        return await _dbSet
            .Include(a => a.Address)
            .Include(a => a.AvailabilityPeriods)
            .Include(a => a.Pictures)
            .FirstOrDefaultAsync(a => a.ExternalId == externalId);
    }

    public async Task<IEnumerable<Accommodation>> Search(AvailabilityFilterDto filter)
    {
        var query = _dbSet
            .Include(a => a.Address)
            .Include(a => a.Pictures)
            .Include(a => a.AvailabilityPeriods)
            .AsQueryable();

        var role = userContext.Role;
        var user = userContext.Name;
        if(role == "HOST") {
            query = query.Where(a => a.Owner == user);
        }


        if (filter == null)
            return await query.ToListAsync();
        
        // Filter by address
        if (!string.IsNullOrWhiteSpace(filter.Address))
        {
            string addressLower = filter.Address.ToLower();
            query = query.Where(a =>
                a.Address.City.ToLower().Contains(addressLower) ||
                a.Address.Country.ToLower().Contains(addressLower));
        }

        // Filter by number of guests
        if (filter.NumberOfGuests > 0)
        {
            query = query.Where(a =>
                (!a.MinNumberOfGuests.HasValue || a.MinNumberOfGuests <= filter.NumberOfGuests) &&
                (!a.MaxNumberOfGuests.HasValue || a.MaxNumberOfGuests >= filter.NumberOfGuests));
        }

        // Filter by start date if provided
        if (!string.IsNullOrWhiteSpace(filter.StartDate))
        {
            if (DateTime.TryParse(filter.StartDate, out DateTime start))
            {
                query = query.Where(a => a.AvailabilityPeriods.Any(p => p.StartDate <= start && p.EndDate >= start));
            }
            else
            {
                throw new ArgumentException("Invalid start date format.");
            }
        }

        // Filter by end date if provided
        if (!string.IsNullOrWhiteSpace(filter.EndDate))
        {
            if (DateTime.TryParse(filter.EndDate, out DateTime end))
            {
                query = query.Where(a => a.AvailabilityPeriods.Any(p => p.StartDate <= end && p.EndDate >= end));
            }
            else
            {
                throw new ArgumentException("Invalid end date format.");
            }
        }


        return await query.ToListAsync();
    }


}