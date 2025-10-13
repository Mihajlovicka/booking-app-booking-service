using BookingService.Model.Messages;

namespace BookingService.Service.MessagingService;
public static class TopicTypeMap
{
    public static readonly Dictionary<KafkaTopic, Type> Map = new()
    {
        { KafkaTopic.AccommodationCreated, typeof(AccommodationCreatedDto) }
    };
}

public enum KafkaTopic
{
    AccommodationCreated
}
