using BookingService.Mapper;
using BookingService.Model.Dto;
using BookingService.Repository.Contract;
using BookingService.Service.Contract;

namespace BookingService.Service.Implementation;

public class ReservationService(IMapperManager mapperManager, IRepositoryManager repositoryManager) : IReservationService
{
    public async Task<IEnumerable<ReservationDto>> GetByAccommodation(string accommodationId)
    {
        var periods = await repositoryManager.ReservationRepository.GetAllReservationsForAccommodation(accommodationId);
        return periods.Select(p => mapperManager.ReservationToReservationDtoMapper.Map(p));
    }    
}