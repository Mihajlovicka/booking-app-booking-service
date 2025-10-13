using BookingService.Mapper;
using BookingService.Model.Entity;
using BookingService.Model.Messages;
using BookingService.Repository.Contract;
using BookingService.Service.MessagingService;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace BookingService.Service.MessagingService;

public class ConsumerService : BackgroundService
{
    private readonly ILogger<ConsumerService> _logger;
    private readonly IConsumer<Ignore, string> _consumer;
    private readonly KafkaTopic _topicName;
    private readonly IServiceScopeFactory _scopeFactory;

    public ConsumerService(
        ILogger<ConsumerService> logger,
        IOptions<ConsumerConfig> config,
        KafkaTopic topicName,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _topicName = topicName;
        _scopeFactory = scopeFactory;
        _consumer = new ConsumerBuilder<Ignore, string>(config.Value).Build();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe(_topicName.ToString());

        Task.Run(async () =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();

                    var _repositoryManager = scope.ServiceProvider.GetRequiredService<IRepositoryManager>();
                    var _mapperManager = scope.ServiceProvider.GetRequiredService<IMapperManager>();

                    var consumeResult = _consumer.Consume(TimeSpan.FromSeconds(5));
                    if (consumeResult is null)
                        continue;

                    _logger.LogInformation($"Kafka message received: {consumeResult.Message.Value}");

                    var type = TopicTypeMap.Map.GetValueOrDefault(_topicName)
                               ?? throw new InvalidOperationException($"No type map found for topic {_topicName}");

                    var message = JsonConvert.DeserializeObject(consumeResult.Message.Value, type);

                    if (message is not AccommodationCreatedDto accommodationDto)
                    {
                        _logger.LogWarning("Message was not a AccommodationCreatedDto. Skipping...");
                        continue;
                    }

                    var accommodation = _mapperManager.AccommodationToAccommodationCreatedDtoMapper.Map(accommodationDto);
                    var existingAcc = await _repositoryManager.AccommodationRepository.GetByExternalIdAsync(accommodation.ExternalId);
                    if (existingAcc != null){
                        existingAcc.PriceType = accommodation.PriceType;
                        await _repositoryManager.AccommodationRepository.UpdateAsync(existingAcc);
                    }
                    else
                    {
                        await _repositoryManager.AccommodationRepository.AddAsync(accommodation);
                    }
                    _logger.LogInformation($"Accommodation '{accommodationDto.Id}' saved successfully!");
                }
                catch (OperationCanceledException)
                {
                    // shutdown signal, safe to ignore
                }
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
