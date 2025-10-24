using BookingService.Mapper;
using BookingService.Model.Dto;
using BookingService.Model.Entity;
using BookingService.Repository.Contract;
using BookingService.Service.Contract;

namespace BookingService.Service.Implementation;

public class AvailabilityService(
        IRepositoryManager repositoryManager,
        IMapperManager mapper
    ) : IAvailabilityService
{
    public async Task<IEnumerable<AccommodationDto>> Search(AvailabilityFilterDto? availabilityFilterDto)
    {
        var accommodations = await repositoryManager.AccommodationRepository.Search(availabilityFilterDto);
        return  await Task.WhenAll(accommodations.Select(async p => await mapper.AccommodationToAccommodationDtoMapper.Map(p)));
    }
    
    public async Task<IEnumerable<AvailabilityPeriodDto>> GetByAccommodation(string accommodationId,  bool? fromToday=null)
    {
        var periods = await repositoryManager.AvailabilityPeriodRepository.GetByAccommodation(accommodationId, fromToday);
        return await Task.WhenAll(periods.Select(async p => await mapper.AvailabilityPeriodToAvailabilityPeriodDtoMapper.Map(p)));
    }

    public async Task<AvailabilityPeriodDto> Add(string accommodationId, AvailabilityPeriodDto dto)
    {
        var start = DateTime.Parse(dto.StartDate);
        var end = DateTime.Parse(dto.EndDate);

        if (repositoryManager.AvailabilityPeriodRepository.Overlaps(accommodationId, dto.Id, start, end))
            throw new InvalidOperationException("Period overlaps with existing availability.");

        var accommodation = await repositoryManager.AccommodationRepository.GetByExternalIdAsync(accommodationId);

        var period = new AvailabilityPeriod
        {
            Accommodation = accommodation,
            StartDate = start,
            EndDate = end,
            Price = dto.Price,
        };
        if(dto.Id.HasValue)
        {
            period.Id = dto.Id.Value;
            await repositoryManager.AvailabilityPeriodRepository.UpdateAsync(period);
        }
        else
        {
            await repositoryManager.AvailabilityPeriodRepository.AddAsync(period);
        }

        return await mapper.AvailabilityPeriodToAvailabilityPeriodDtoMapper.Map(period);
    }

    public async Task Delete(int id)
    {
        var period = await repositoryManager.AvailabilityPeriodRepository.GetByIdAsync(id);
        if (period == null)
            throw new KeyNotFoundException("Availability period not found.");

        await repositoryManager.AvailabilityPeriodRepository.DeleteAsync(id);
    }

    public async Task<UserDeleteCheckDto> UserDeleteCheckStatus()
    {
        int count = await repositoryManager.ReservationRepository.GetFutureReservationCountAsync();
        var d = new UserDeleteCheckDto
        {
            requestDenied = count > 0
        };
        return d;
    }

}