using Moq;
using BookingService.Mapper;
using BookingService.Model.Dto;
using BookingService.Model.Entity;
using BookingService.Repository.Contract;
using BookingService.Service.Contract;


namespace BookingService.Tests;

[TestFixture]
[Category("Unit")]
public class AvailabilityServiceTests
{
    private Mock<IRepositoryManager> _mockRepositoryManager;
    private Mock<IUserContext> _mockUserContext;
    private Mock<IMapperManager> _mockMapperManager;
    private IAvailabilityService _availabilityService;

    [SetUp]
    public void Setup()
    {
        _mockRepositoryManager = new Mock<IRepositoryManager>();
        _mockUserContext = new Mock<IUserContext>();
        _mockMapperManager = new Mock<IMapperManager>();

        _availabilityService = new BookingService.Service.Implementation.AvailabilityService(
            _mockRepositoryManager.Object,
            _mockMapperManager.Object
        );
    }

    [Test]
    public async Task GetByAccommodation_ReturnsMappedPeriods()
    {
        // Arrange
        string accommodationId = "acc123";
        var periods = new List<AvailabilityPeriod>
            {
                new AvailabilityPeriod { Id = 1, StartDate = DateTime.Today, EndDate = DateTime.Today.AddDays(2), Price = 100 }
            };

        var mappedPeriods = new List<AvailabilityPeriodDto>
            {
                new AvailabilityPeriodDto { Id = 1, StartDate = DateTime.Today.ToString(), EndDate = DateTime.Today.AddDays(2).ToString(), Price = 100 }
            };

        _mockRepositoryManager
            .Setup(r => r.AvailabilityPeriodRepository.GetByAccommodation(accommodationId, null))
            .ReturnsAsync(periods);

        _mockMapperManager
            .Setup(m => m.AvailabilityPeriodToAvailabilityPeriodDtoMapper.Map(It.IsAny<AvailabilityPeriod>()))
            .ReturnsAsync((AvailabilityPeriod p) => mappedPeriods.First(mp => mp.Id == p.Id));


        // Act
        var result = await _availabilityService.GetByAccommodation(accommodationId);

        // Assert
        Assert.AreEqual(1, result.Count());
        Assert.AreEqual(periods[0].Id, result.First().Id);
    }

    [Test]
    public void Add_OverlappingPeriod_ThrowsInvalidOperationException()
    {
        // Arrange
        string accommodationId = "acc123";
        var dto = new AvailabilityPeriodDto { StartDate = "2025-10-14", EndDate = "2025-10-16", Price = 100 };

        _mockRepositoryManager
            .Setup(r => r.AvailabilityPeriodRepository.Overlaps(accommodationId, dto.Id, DateTime.Parse(dto.StartDate), DateTime.Parse(dto.EndDate)))
            .Returns(true);

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(
            async () => await _availabilityService.Add(accommodationId, dto)
        );
        Assert.That(ex.Message, Is.EqualTo("Period overlaps with existing availability."));
    }

    [Test]
    public async Task Add_NewPeriod_AddsSuccessfully()
    {
        // Arrange
        string accommodationId = "acc123";
        var dto = new AvailabilityPeriodDto { StartDate = "2025-10-14", EndDate = "2025-10-16", Price = 100 };

        var accommodation = new Accommodation { ExternalId = accommodationId };
        var period = new AvailabilityPeriod { StartDate = DateTime.Parse(dto.StartDate), EndDate = DateTime.Parse(dto.EndDate), Price = dto.Price, Accommodation = accommodation };

        _mockRepositoryManager.Setup(r => r.AvailabilityPeriodRepository.Overlaps(accommodationId, dto.Id, period.StartDate, period.EndDate))
                              .Returns(false);

        _mockRepositoryManager.Setup(r => r.AccommodationRepository.GetByExternalIdAsync(accommodationId))
                              .ReturnsAsync(accommodation);

        _mockMapperManager.Setup(m => m.AvailabilityPeriodToAvailabilityPeriodDtoMapper.Map(It.IsAny<AvailabilityPeriod>()))
                          .ReturnsAsync(dto);

        // Act
        var result = await _availabilityService.Add(accommodationId, dto);

        // Assert
        _mockRepositoryManager.Verify(r => r.AvailabilityPeriodRepository.AddAsync(It.IsAny<AvailabilityPeriod>()), Times.Once);
        Assert.AreEqual(dto.Price, result.Price);
    }

    [Test]
    public void Delete_NonExistingPeriod_ThrowsKeyNotFoundException()
    {
        // Arrange
        int periodId = 1;
        _mockRepositoryManager.Setup(r => r.AvailabilityPeriodRepository.GetByIdAsync(periodId))
                              .ReturnsAsync((AvailabilityPeriod)null);

        // Act & Assert
        var ex = Assert.ThrowsAsync<KeyNotFoundException>(
            async () => await _availabilityService.Delete(periodId)
        );
        Assert.That(ex.Message, Is.EqualTo("Availability period not found."));
    }

    [Test]
    public async Task Delete_ExistingPeriod_DeletesSuccessfully()
    {
        // Arrange
        int periodId = 1;
        var period = new AvailabilityPeriod { Id = periodId };
        _mockRepositoryManager.Setup(r => r.AvailabilityPeriodRepository.GetByIdAsync(periodId))
                              .ReturnsAsync(period);

        // Act
        await _availabilityService.Delete(periodId);

        // Assert
        _mockRepositoryManager.Verify(r => r.AvailabilityPeriodRepository.DeleteAsync(periodId), Times.Once);
    }
}