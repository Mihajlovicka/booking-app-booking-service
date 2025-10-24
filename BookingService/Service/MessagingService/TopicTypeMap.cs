using BookingService.Model.Messages;

namespace BookingService.Service.MessagingService;
public static class TopicTypeMap
{
    public static readonly Dictionary<KafkaTopic, Type> Map = new()
    {
        { KafkaTopic.AccommodationCreated, typeof(AccommodationCreatedDto) },
        { KafkaTopic.UserCreated, typeof(UserDto)},
        { KafkaTopic.DeleteUser, typeof(UserDto)},
        { KafkaTopic.NotificationCreated, typeof(NotificationDto) }
    };
}

public enum KafkaTopic
{
    AccommodationCreated,
    UserCreated,
    DeleteUser,
    NotificationCreated
}
