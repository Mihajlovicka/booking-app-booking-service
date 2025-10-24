using BookingService.Model.Dto; 

namespace BookingService.Service.Contract;

public interface IAvailabilityService
{
    Task<IEnumerable<AccommodationDto>> Search(AvailabilityFilterDto? availabilityFilterDto);
    Task<IEnumerable<AvailabilityPeriodDto>> GetByAccommodation(string accommodationId, bool? fromToday=null);
    Task<AvailabilityPeriodDto> Add(string accommodationId, AvailabilityPeriodDto dto);
    Task Delete(int id);
    Task<UserDeleteCheckDto> UserDeleteCheckStatus();
}