using BookingService.Mapper;
using BookingService.Model.Messages;
using BookingService.Repository.Contract;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace BookingService.Service.MessagingService;

public class ConsumerService : BackgroundService
{
    private readonly ILogger<ConsumerService> _logger;
    private readonly IConsumer<Ignore, string> _consumer;
    private readonly IEnumerable<KafkaTopic> _topics;
    private readonly IServiceScopeFactory _scopeFactory;

    public ConsumerService(
        ILogger<ConsumerService> logger,
        IOptions<ConsumerConfig> config,
        IEnumerable<KafkaTopic> topics,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _topics = topics;
        _scopeFactory = scopeFactory;
        _consumer = new ConsumerBuilder<Ignore, string>(config.Value).Build();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe(_topics.Select(t => t.ToString()).ToList());

        Task.Run(async () =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();

                    var repositoryManager = scope.ServiceProvider.GetRequiredService<IRepositoryManager>();
                    var mapperManager = scope.ServiceProvider.GetRequiredService<IMapperManager>();

                    var consumeResult = _consumer.Consume(TimeSpan.FromSeconds(5));
                    if (consumeResult is null)
                        continue;

                    var topic = consumeResult.Topic;
                    _logger.LogInformation($"Kafka message received on topic {topic}: {consumeResult.Message.Value}");

                    var topicEnum = Enum.Parse<KafkaTopic>(topic);

                    var type = TopicTypeMap.Map.GetValueOrDefault(topicEnum)
                               ?? throw new InvalidOperationException($"No type map found for topic {topic}");

                    var message = JsonConvert.DeserializeObject(consumeResult.Message.Value, type);

                    switch (message)
                    {
                        case AccommodationCreatedDto accommodationDto:
                            await HandleAccommodationCreated(accommodationDto, repositoryManager, mapperManager);
                            break;

                        case UserDto userDto:
                            await HandleUserCreated(userDto, repositoryManager, mapperManager);
                            break;

                        default:
                            _logger.LogWarning($"Unknown message type received for topic {topic}");
                            break;
                    }
                }
                catch (OperationCanceledException) { }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while consuming Kafka message");
                }
            }
        }, stoppingToken);

        return Task.CompletedTask;
    }

    private async Task HandleAccommodationCreated(AccommodationCreatedDto dto, IRepositoryManager repo, IMapperManager mapper)
    {
        var accommodation = mapper.AccommodationToAccommodationCreatedDtoMapper.Map(dto);
        var existing = await repo.AccommodationRepository.GetByExternalIdAsync(accommodation.ExternalId);

        if (existing != null)
        {
            existing.PriceType = accommodation.PriceType;
            await repo.AccommodationRepository.UpdateAsync(existing);
        }
        else
        {
            await repo.AccommodationRepository.AddAsync(accommodation);
        }
        _logger.LogInformation($"Accommodation '{dto.Id}' processed.");
    }

    private async Task HandleUserCreated(UserDto dto, IRepositoryManager repo, IMapperManager mapper)
    {
        var user = mapper.UserDtoToUserMapper.Map(dto);
        await repo.UserRepository.AddAsync(user);

        _logger.LogInformation($"User '{dto.Username}' saved in BookingService.");
    }

    public override void Dispose()
    {
        _consumer.Close();
        _consumer.Dispose();
        base.Dispose();
    }
}
