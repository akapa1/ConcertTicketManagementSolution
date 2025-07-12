namespace ConcertTicketManagementSystem.Tests.Controllers;

using ConcertTicketManagementSystem.Controllers;
using ConcertTicketManagementSystem.Interfaces;
using ConcertTicketManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

[TestFixture]
public class EventsControllerTests
{
    private Mock<IEventService> _mockService;
    private EventsController _controller;

    [SetUp]
    public void SetUp()
    {
        _mockService = new Mock<IEventService>();
        _controller = new EventsController(_mockService.Object);
    }

    [Test]
    public async Task GetAllAsync_ReturnsAllEvents()
    {
        // Arrange
        var events = new List<Event> { new Event {
            Venue = "Test Venue",
            Description = "Test Concert",
            Capacity = 100,
            Date = new DateTime(2027,6,7),
            TicketTypes = new List<TicketType>
            {
                new TicketType { Id = 1, TypeName = "VIP", QuantityAvailable = 40 },
                new TicketType { Id = 2, TypeName = "General", QuantityAvailable = 50 }
            }
        }};
        _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(events);

        // Act
        var result = await _controller.GetAllAsync();

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        Assert.That(okResult.Value, Is.EqualTo(events));
    }

    [Test]
    public async Task GetByIdAsync_ReturnsEvent_WhenFound()
    {
        // Arrange
        var evt = new Event {
            Venue = "Test Venue",
            Description = "Test Concert",
            Capacity = 100,
            Date = new DateTime(2027,6,7),
            TicketTypes = new List<TicketType>
            {
                new TicketType { Id = 1, TypeName = "VIP", QuantityAvailable = 40 },
                new TicketType { Id = 2, TypeName = "General", QuantityAvailable = 50 }
            }
        };
        _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(evt);

        // Act
        var result = await _controller.GetByIdAsync(1);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult?.Value, Is.EqualTo(evt));
    }

    [Test]
    public async Task GetByIdAsync_ReturnsNotFound_WhenNotFound()
    {
        // Arrange
        _mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((Event)null);

        // Act
        var result = await _controller.GetByIdAsync(99);

        // Assert
        Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task CreateAsync_ReturnsCreatedEvent()
    {
        // Arrange
        var newEvent = new Event {
            Venue = "New Venue",
            Description = "New Concert",
            Capacity = 100,
            Date = new DateTime(2027,6,7),
            TicketTypes = new List<TicketType>
            {
                new TicketType { Id = 1, TypeName = "VIP", QuantityAvailable = 40 },
                new TicketType { Id = 2, TypeName = "General", QuantityAvailable = 50 }
            }
        };
        _mockService.Setup(s => s.CreateAsync(newEvent)).ReturnsAsync(newEvent);

        // Act
        var result = await _controller.CreateAsync(newEvent);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Value, Is.EqualTo(newEvent));
    }

    [Test]
    public async Task UpdateAsync_ReturnsNoContent_WhenSuccessful()
    {
        // Arrange
        var updated = new Event {
            Venue = "Test Venue Updated",
            Description = "Test Concert Updated",
            Capacity = 100,
            Date = new DateTime(2027,6,7),
            TicketTypes = new List<TicketType>
            {
                new TicketType { Id = 1, TypeName = "VIP", QuantityAvailable = 40 },
                new TicketType { Id = 2, TypeName = "General", QuantityAvailable = 50 }
            }
        };
        _mockService.Setup(s => s.UpdateAsync(1, updated)).ReturnsAsync(true);

        // Act
        var result = await _controller.UpdateAsync(1, updated);

        // Assert
        Assert.That(result, Is.TypeOf<NoContentResult>());
    }

    [Test]
    public async Task UpdateAsync_ReturnsNotFound_WhenFailed()
    {
        // Arrange
        var updated = new Event {
            Venue = "Test Venue",
            Description = "Test Concert",
            Capacity = 100,
            Date = new DateTime(2027,6,7),
            TicketTypes = new List<TicketType>
            {
                new TicketType { Id = 1, TypeName = "VIP", QuantityAvailable = 40 },
                new TicketType { Id = 2, TypeName = "General", QuantityAvailable = 50 }
            }
        };
        _mockService.Setup(s => s.UpdateAsync(1, updated)).ReturnsAsync(false);

        // Act
        var result = await _controller.UpdateAsync(1, updated);

        // Assert
        Assert.That(result, Is.TypeOf<NotFoundResult>());
    }
}

