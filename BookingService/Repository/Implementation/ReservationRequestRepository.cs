using BookingService.Data;
using BookingService.Model.Entity;
using BookingService.Repository.Contract;

namespace BookingService.Repository.Implementation;

public class ReservationRequestRepository(AppDbContext context)
    : CrudRepository<ReservationRequest>(context), IReservationRequestRepository;