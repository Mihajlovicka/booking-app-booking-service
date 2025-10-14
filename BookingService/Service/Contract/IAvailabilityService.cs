using BookingService.Model.Dto;
using BookingService.Model.Entity;

namespace BookingService.Service.Contract;

public interface IAvailabilityService
{
    Task<IEnumerable<AccommodationDto>> Search(AvailabilityFilterDto? availabilityFilterDto);
    Task<IEnumerable<AvailabilityPeriodDto>> GetByAccommodation(string accommodationId);
    Task<AvailabilityPeriodDto> Add(string accommodationId, AvailabilityPeriodDto dto);
    Task Delete(int id);
}