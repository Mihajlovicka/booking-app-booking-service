using System.Runtime.InteropServices.JavaScript;
using BookingService.Mapper;
using BookingService.Model.Dto;
using BookingService.Model.Entity;
using BookingService.Model.Messages;
using BookingService.Repository.Contract;
using BookingService.Service.Contract;
using BookingService.Service.MessagingService;

namespace BookingService.Service.Implementation;

public class ReservationService(
    IMapperManager mapperManager,
    IRepositoryManager repositoryManager,
    ProducerService producerService,
    IUserContext userContext)
    : IReservationService
{
    public async Task<IEnumerable<ReservationDto>> GetByAccommodation(string accommodationId)
    {
        var reservations =
            await repositoryManager.ReservationRepository.GetAllReservationsForAccommodation(accommodationId);
        return await Task.WhenAll(reservations.Select(p => mapperManager.ReservationToReservationDtoMapper.Map(p)));
    }

    public async Task CreateReservation(ReservationRequest request)
    {
        var accommodation =
            await repositoryManager.AccommodationRepository.GetByExternalIdAsync(request.Accommodation.ExternalId);

        if (accommodation is null) throw new Exception("Accommodation does not exists");

        if (repositoryManager.ReservationRepository.Overlaps(accommodation.ExternalId, request.StartDate,
                request.EndDate))
        {
            throw new Exception("For this period reservation already exists!");
        }

        Reservation reservation = new()
        {
            FinalPrice = request.FinalPrice,
            GuestNumber = request.GuestNumber,
            GuestUsername = request.Guest.Username,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Accommodation = accommodation,
            ExternalId = Guid.NewGuid()
        };

        await repositoryManager.ReservationRepository.AddAsync(reservation);
    }

    public async Task<IEnumerable<ReservationDto>> GetMy(string username)
    {
        var reservations = await repositoryManager.ReservationRepository.GetMy(username);

        List<ReservationDto> result = [];

        foreach (var reservation in reservations)
        {
            result.Add(await mapperManager.ReservationToReservationDtoMapper.Map(reservation));
        }

        return result;
    }

    public async Task Cancel(int reservationId)
    {
        var reservation = await repositoryManager.ReservationRepository.GetByIdAsyncWithAccomodation(reservationId);

        if (reservation is null) throw new Exception("Reservation does not exist");
        
        if(!reservation.Active) throw new Exception("Reservation does not exist");

        var todayUtc = DateTime.Today;

        if (reservation.StartDate.Date <= todayUtc) 
        {
            throw new Exception("Cannot cancel a reservation that has already started");
        }

        reservation.Active = false;

        await repositoryManager.ReservationRepository.UpdateAsync(reservation);

        var role = userContext.Role;
        if(role == Role.GUEST.ToString())
        {
            var user = await repositoryManager.UserRepository.GetByUsernameAsync(reservation.Accommodation.Owner);
            var notificationDto = new NotificationDto
            {
                NotificationTypeId = (int)NotificationType.CancelReservation,
                NotificationUserExternalId = user.ExternalId,
                Message = "Reservation " + reservation.Accommodation.Name +" canceled"
            };

            await producerService.ProduceAsync<NotificationDto>(KafkaTopic.NotificationCreated.ToString(), notificationDto);
        }

    }
}