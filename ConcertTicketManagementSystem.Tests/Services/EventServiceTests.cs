namespace ConcertTicketManagementSystem.Tests.Services;

using Moq;
using ConcertTicketManagementSystem.Interfaces;
using ConcertTicketManagementSystem.Models;
using ConcertTicketManagementSystem.Services;
using Microsoft.AspNetCore.Http.HttpResults;

[TestFixture]
public class EventServiceTests
{
    private Mock<IEventRepository> _mockEventRepo;
    private EventService _eventService;

    [SetUp]
    public void SetUp()
    {
        _mockEventRepo = new Mock<IEventRepository>();
        _eventService = new EventService(_mockEventRepo.Object);
    }

    [Test]
    public async Task GetAllAsync_ShouldReturnAllEvents()
    {
        // Arrange
        var events = new List<Event> {
            new Event { Id = 1, Venue = "Venue A", Description = "Katy Perry Concert", Date = new DateTime(2026,4,5) },
            new Event { Id = 2, Venue = "Venue A", Description = "Beyonce Concert", Date = new DateTime(2026,5,5)}
        };

        _mockEventRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(events);

        // Act
        var result = await _eventService.GetAllAsync();

        // Assert
        Assert.That(((List<Event>)result).Count, Is.EqualTo(2));
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnEvent_WhenExists()
    {
        // Arrange
        var evt = new Event { Id = 1, Venue = "Venue A", Description = "Katy Perry Concert", Date = new DateTime(2026,4,5)};
        _mockEventRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(evt);

        // Act
        var result = await _eventService.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.That(result.Venue, Is.EqualTo("Venue A"));
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        // Arrange
        _mockEventRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Event?)null);

        // Act
        var result = await _eventService.GetByIdAsync(99);

        // Assert
        Assert.Null(result);
    }

    [Test]
    public async Task CreateAsync_ShouldReturnCreatedEvent()
    {
        // Arrange
        var newEvent = new Event { Id = 1, Venue = "Venue A", Description = "Katy Perry Concert", Date = new DateTime(2026,4,5) };
        var createdEvent = new Event { Id = 1, Venue = "Venue A", Description = "Beyonce Concert", Date = new DateTime(2026,5,5) };

        _mockEventRepo.Setup(r => r.CreateAsync(newEvent)).ReturnsAsync(createdEvent);

        // Act
        var result = await _eventService.CreateAsync(newEvent);

        // Assert
        Assert.That(result.Id, Is.EqualTo(1));
        Assert.That(result.Description, Is.EqualTo("Beyonce Concert"));
    }

    [Test]
    public async Task UpdateAsync_ShouldReturnTrue_WhenEventUpdated()
    {
        // Arrange
        var updatedEvent = new Event { Id = 1, Venue = "Venue A", Description = "Katy Perry Concert", Date = new DateTime(2027,5,5) };
        _mockEventRepo.Setup(r => r.UpdateAsync(1, updatedEvent)).ReturnsAsync(true);

        // Act
        var result = await _eventService.UpdateAsync(1, updatedEvent);

        // Assert
        Assert.True(result);
    }

    [Test]
    public async Task UpdateAsync_ShouldReturnFalse_WhenEventNotFound()
    {
        // Arrange
        var updatedEvent = new Event { Id = 56, Venue = "Some Venue", Description = "Someones Concert", Date = new DateTime(2027,5,5) };
        _mockEventRepo.Setup(r => r.UpdateAsync(99, updatedEvent)).ReturnsAsync(false);

        // Act
        var result = await _eventService.UpdateAsync(99, updatedEvent);

        // Assert
        Assert.False(result);
    }
}
