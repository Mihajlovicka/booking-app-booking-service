using BookingService.Mapper;
using BookingService.Model.Dto;
using BookingService.Repository.Contract;
using BookingService.Service.Contract;

namespace BookingService.Service.Implementation;

public class ReservationService(IMapperManager mapperManager, IRepositoryManager repositoryManager) : IReservationService
{
    public async Task<IEnumerable<ReservationDto>> GetByAccommodation(string accommodationId)
    {
        var reservations = await repositoryManager.ReservationRepository.GetAllReservationsForAccommodation(accommodationId);
        return await Task.WhenAll(reservations.Select(p => mapperManager.ReservationToReservationDtoMapper.Map(p)));
    }
}