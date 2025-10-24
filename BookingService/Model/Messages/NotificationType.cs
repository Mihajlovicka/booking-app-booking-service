namespace BookingService.Model.Messages;
public enum NotificationType
{
    CreateReservation = 1, //guest kreira
    CancelReservation = 2, //guest otkazuje rezervaciju
    NewRateOnHost = 3,
    NewRateOnAccommodation = 4,
    ReservationProcessed = 5 //Host odgovori na zahtev za rezervaciju odobri otkaze automatski se kreira
}
