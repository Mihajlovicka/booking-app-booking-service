using System.Threading.Tasks;
using BookingService.Mapper;
using BookingService.Model.Dto;
using BookingService.Model.Entity;
using BookingService.Repository.Contract;
using BookingService.Service.Contract;

namespace BookingService.Service.Implementation;

public class AvailabilityService(
        IRepositoryManager repositoryManager,
        IUserContext userContext,
        IMapperManager mapper
    ) : IAvailabilityService
{
    public async Task<IEnumerable<AvailabilityPeriodDto>> GetByAccommodation(string accommodationId)
    {
        var periods = await repositoryManager.AvailabilityPeriodRepository.GetByAccommodation(accommodationId);
        return periods.Select(p => mapper.AvailabilityPeriodToAvailabilityPeriodDtoMapper.Map(p));
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

        return mapper.AvailabilityPeriodToAvailabilityPeriodDtoMapper.Map(period);
    }

    public async Task Delete(int id)
    {
        var period = await repositoryManager.AvailabilityPeriodRepository.GetByIdAsync(id);
        if (period == null)
            throw new KeyNotFoundException("Availability period not found.");

        await repositoryManager.AvailabilityPeriodRepository.DeleteAsync(id);
    }

}