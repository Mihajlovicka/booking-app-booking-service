using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using BookingService.Data;
using BookingService.Model.Dto;
using BookingService.Model.Entity;
using BookingService.Service.MessagingService;
using NUnit.Framework;
using BookingService.Model.Messages;
using Microsoft.Extensions.Configuration;
using Confluent.Kafka;
using System.Net.Http.Headers;

namespace BookingService.Tests;

[TestFixture]
[Category("Integration")]
public class DeleteUserIntegrationTests
{
    private HttpClient _client;
    private CustomWebApplicationFactory _factory;
    private string KafkaBroker = "localhost:9092";

    private Guid uuid = Guid.NewGuid();

    [OneTimeSetUp]
    public async Task Setup()
    {
        _factory = new CustomWebApplicationFactory();
        _client = _factory.CreateClient();

        var config = _factory.Services.GetRequiredService<IConfiguration>();
        KafkaBroker = config.GetValue<string>("KafkaConfig:Producer:BootstrapServers");

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

        var user = new UserDto { Id = uuid, Username = "host@example.com", Role = "HOST" };

        await SetupDbData();
        await ProduceKafkaMessage<UserDto>(KafkaTopic.DeleteUser.ToString(), user);

    }

    [OneTimeTearDown]
    public void TearDown()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [Test]
    [Order(0)]
    public async Task CanDelete_Yes()
    {

        var getResponse = await _client.GetAsync($"/api/user/delete-check");
        getResponse.EnsureSuccessStatusCode();
        var res = await getResponse.Content.ReadFromJsonAsync<UserDeleteCheckDto>();
        Assert.That(res.requestDenied, Is.False);
        
    }

    [Test]
    [Order(2)]
    public async Task GetUser_NoUser()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        Assert.That(db.Accommodations.Count() == 0);
        Assert.That(db.Users.Count() == 0);
    }

    [Test]
    [Order(3)]
    public async Task CanDelete_No()
    {
        await SetupDbData();

        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var accommodation = db.Accommodations.First();

        var reseivation = new Reservation
        {
            AccommodationId = 0,
            Accommodation = accommodation,
            StartDate = DateTime.Now.AddDays(1),
            EndDate = DateTime.Now.AddDays(5),
            ExternalId = Guid.NewGuid(),
            FinalPrice = 50,
            GuestNumber = 5,
            GuestUsername = "guest",
Active = true
        };

        db.Reservations.Add(reseivation);
        db.SaveChanges();

        var getResponse = await _client.GetAsync($"/api/user/delete-check");
        getResponse.EnsureSuccessStatusCode();
        var res = await getResponse.Content.ReadFromJsonAsync<UserDeleteCheckDto>();
        Assert.That(res.requestDenied, Is.True);
    }


    private async Task SetupDbData()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var user = new User { Id = 0, ExternalId = Guid.NewGuid(), Username = "host@example.com", Role = Role.HOST };
        db.Users.Add(user);
        db.SaveChanges();

        var address = new Address
        {
            City = "city",
            Country = "country",
            PostNumber = "post num",
            StreetName = "StreetName",
            StreetNumber = "StreetNumber"
        };

        var accommodation = new Accommodation { Id = 0, Name = "akomodacija", ExternalId = Guid.NewGuid().ToString(), Address = address, Owner = user.Username };
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

}
