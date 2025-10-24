namespace BookingService.Model.Messages;
public class NotificationDto
{
    public Guid NotificationUserExternalId { get; set; }
    public string Message { get; set; }
    public int NotificationTypeId { get; set; }
}