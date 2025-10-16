using BookingService.Mapper;
using BookingService.Model.Entity;
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
                        {
                            var accommodation =
                                await mapperManager.AccommodationToAccommodationCreatedDtoMapper.Map(accommodationDto);
                            var existingAcc =
                                await repositoryManager.AccommodationRepository.GetByExternalIdAsync(accommodation
                                    .ExternalId);
                            if (existingAcc != null)
                            {
                                existingAcc.PriceType = accommodation.PriceType;
                                await repositoryManager.AccommodationRepository.UpdateAsync(existingAcc);
                            }
                            else
                            {
                                var address = new Address
                                {
                                    StreetNumber = accommodationDto.Address.StreetNumber,
                                    StreetName = accommodationDto.Address.StreetName,
                                    City = accommodationDto.Address.City,
                                    PostNumber = accommodationDto.Address.PostNumber,
                                    Country = accommodationDto.Address.Country
                                };
                                accommodation.Address = address;
                                await repositoryManager.AccommodationRepository.AddAsync(accommodation);
                            }

                            _logger.LogInformation($"Accommodation '{accommodationDto.Id}' saved successfully!");
                            break;
                        }
                        case UserDto userDto:
                            var user = await mapperManager.UserDtoToUserMapper.Map(userDto);
                            await repositoryManager.UserRepository.AddAsync(user);

                            _logger.LogInformation($"User '{userDto.Username}' saved in BookingService.");
                            break;
                        default:
                            _logger.LogWarning($"Unknown message type received for topic {topic}");
                            break;
                    }
                }
                catch (OperationCanceledException)
                { }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while consuming Kafka message");
                }
            }
        }, stoppingToken);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _consumer.Close();
        _consumer.Dispose();
        base.Dispose();
    }
}