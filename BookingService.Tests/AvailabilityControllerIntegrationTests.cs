using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using BookingService.Data;
using BookingService.Model.Dto;
using BookingService.Model.Entity;
using BookingService.Model.Messages;
using BookingService.Service.MessagingService;
using Confluent.Kafka;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mysqlx.Crud;

namespace BookingService.Tests;

[TestFixture]
[Category("Integration")]
public class AvailabilityControllerIntegrationTests
{
    private HttpClient _client;
    private CustomWebApplicationFactory _factory;
    private string KafkaBroker = "localhost:29092";
    private Guid accommodationId;

    [OneTimeSetUp]
    public async Task Setup()
    {
        _factory = new CustomWebApplicationFactory();
        _client = _factory.CreateClient();
        await SetupDbData();

        var config = _factory.Services.GetRequiredService<IConfiguration>();
        KafkaBroker = config.GetValue<string>("KafkaConfig:Producer:BootstrapServers");
    

    }

    [OneTimeTearDown]
    public void TearDown()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [Test]
    public async Task AddAvailabilityPeriod_IntegrationTest()
    {
        // Arrange

        // await ProduceKafkaMessage(KafkaTopic.AccommodationCreated.ToString(), accommodationDto);

        // Wait and consume the message to ensure system processed it
        // var consumedMessage = await ConsumeKafkaMessage(KafkaTopic.AccommodationCreated.ToString());
        // Assert.IsNotNull(consumedMessage);

        // Act - Add availability period
        var availabilityDto = new AvailabilityPeriodDto
        {
            StartDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"),
            EndDate = DateTime.Now.AddDays(6).ToString("yyyy-MM-dd"),
            Price = 200
        };

        var request = new HttpRequestMessage(HttpMethod.Post, $"/api/accommodations/{accommodationId}/availability")
        {
            Content = JsonContent.Create(availabilityDto)
        };

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var resultDto = await response.Content.ReadFromJsonAsync<AvailabilityPeriodDto>();
        Assert.AreEqual(availabilityDto.Price, resultDto.Price);

        // Act - Get all availability periods
        var getResponse = await _client.GetAsync($"/api/accommodations/{accommodationId}/availability");
        getResponse.EnsureSuccessStatusCode();
        var periods = await getResponse.Content.ReadFromJsonAsync<List<AvailabilityPeriodDto>>();
        Assert.IsNotEmpty(periods);

        // Act - Delete availability period
        var periodId = resultDto.Id.Value;
        var deleteRequest = new HttpRequestMessage(HttpMethod.Delete, $"/api/accommodations/{accommodationId}/availability/{periodId}");

        var deleteResponse = await _client.SendAsync(deleteRequest);
        Assert.AreEqual(System.Net.HttpStatusCode.NoContent, deleteResponse.StatusCode);
    }

    [Test]
    public async Task SearchAvailability_IntegrationTest()
    {
        // Act: Search by address and date filter
        var filter = new AvailabilityFilterDto
        {
            Address = "Belgrade",
            StartDate = DateTime.Today.AddDays(11).ToString("yyyy-MM-dd"),
            EndDate = DateTime.Today.AddDays(12).ToString("yyyy-MM-dd"),
            NumberOfGuests = 2
        };

        var response = await _client.PostAsJsonAsync("/api/accommodations/search", filter);
        response.EnsureSuccessStatusCode();

        var accommodations = await response.Content.ReadFromJsonAsync<List<AccommodationDto>>();

        // Assert
        Assert.IsNotNull(accommodations);
        Assert.IsTrue(accommodations.Any());
        Assert.AreEqual("Belgrade", accommodations.First().Address.City);
        Assert.AreEqual("Test Apartment", accommodations.First().Name);
    }


    private async Task SetupDbData()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        accommodationId = Guid.NewGuid();

        // Produce AccommodationCreatedDto Kafka message
        var accommodation = new Accommodation
        {
            ExternalId = accommodationId.ToString(),
            PriceType = Model.Entity.PriceType.PerGuest,
            Owner = "host@example.com",
            Name = "Test Apartment",
            MinNumberOfGuests = 1,
            MaxNumberOfGuests = 5,
            Pictures = [],
            Address = new Address
            {
                StreetNumber = "12A",
                StreetName = "Main Street",
                City = "Belgrade",
                PostNumber = "11000",
                Country = "Serbia"
            },
            AvailabilityPeriods = [

                new AvailabilityPeriod
                    {
                        StartDate = DateTime.Now.AddDays(10),
                        EndDate = DateTime.Now.AddDays(12),
                        Price = 400
                    }
            ]
        };

        db.Accommodations.Add(accommodation);
        db.SaveChanges();

    }

    private async Task ProduceKafkaMessage<T>(string topic, T message)
    {
        var config = new ProducerConfig { BootstrapServers = KafkaBroker };
        using var producer = new ProducerBuilder<Null, string>(config).Build();
        var json = JsonSerializer.Serialize(message);
        await producer.ProduceAsync(topic, new Message<Null, string> { Value = json });
        producer.Flush(TimeSpan.FromSeconds(5));
    }

    private async Task<string> ConsumeKafkaMessage(string topic)
    {
        var config = new ConsumerConfig
            {
                BootstrapServers = KafkaBroker,
                GroupId = $"test-group-{Guid.NewGuid()}",
                AutoOffsetReset = AutoOffsetReset.Earliest,
            };

        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe(topic);

        var timeout = TimeSpan.FromSeconds(10);
        var start = DateTime.Now;

        try
        {
            while (DateTime.Now - start < timeout)
            {
                var result = consumer.Consume(100);
                if (result != null && !string.IsNullOrWhiteSpace(result.Message?.Value))
                {
                    return result.Message.Value;
                }
            }
        }
        finally
        {
            consumer.Close();
        }
        return null; // Timeout if no message is received
    }
}
