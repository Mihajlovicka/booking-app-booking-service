using BookingService.Service.MessagingService;
using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace BookingService.Extensions;

public static class KafkaExtensions
{
    public static IServiceCollection AddKafkaServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // Configure Producer
        services.Configure<ProducerConfig>(configuration.GetSection("KafkaConfig:Producer"));
        services.AddSingleton<ProducerService>();

        // Configure Consumer - listening to multiple topics
        services.Configure<ConsumerConfig>(configuration.GetSection("KafkaConfig:Consumer"));
        services.AddHostedService(provider =>
        {
            var logger = provider.GetRequiredService<ILogger<ConsumerService>>();
            var consumerConfig = provider.GetRequiredService<IOptions<ConsumerConfig>>();
            var scopeFactory = provider.GetRequiredService<IServiceScopeFactory>();

            // Listen to both topics
            var topics = new[]
            {
                KafkaTopic.AccommodationCreated,
                KafkaTopic.UserCreated
            };

            return new ConsumerService(logger, consumerConfig, topics, scopeFactory);
        });

        return services;
    }
}