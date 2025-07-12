namespace ConcertTicketManagementSystem.Tests.Repositories;

using ConcertTicketManagementSystem.Models;
using ConcertTicketManagementSystem.Repositories;
using Microsoft.AspNetCore.Http;

[TestFixture]
public class EventRepositoryTests
{
    private EventRepository _repository;

    [SetUp]
    public void Setup()
    {
        _repository = new EventRepository();
    }

    [Test]
    public async Task CreateAsync_ShouldCreateEvent_WhenTicketQuantityWithinCapacity()
    {
        // Arrange
        var newEvent = new Event
        {
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

        // Act
        var result = await _repository.CreateAsync(newEvent);

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.Id, Is.EqualTo(1));
    }

    [Test]
    public void CreateAsync_ShouldThrowException_WhenTicketQuantityExceedsCapacity()
    {
        // Arrange
        var newEvent = new Event
        {
            Venue = "Test Venue",
            Description = "Test Concert",
            Capacity = 100,
            Date = new DateTime(2027,6,7),
            TicketTypes = new List<TicketType>
            {
                new TicketType { Id = 1, TypeName = "VIP", QuantityAvailable = 51 },
                new TicketType { Id = 2, TypeName = "General", QuantityAvailable = 50 }
            }
        };

        // Act & Assert
        var ex = Assert.ThrowsAsync<BadHttpRequestException>(() => _repository.CreateAsync(newEvent));
        Assert.That(ex.Message, Is.EqualTo("Total ticket quantity exceeds event capacity."));
    }

    [Test]
    public async Task GetAllAsync_ShouldReturnAllEvents()
    {
        // Arrange
        await _repository.CreateAsync(new Event
        {
            Venue = "Venue 1",
            Description = "Event 1",
            Capacity = 100,
            Date = new DateTime(2027,6,7),
            TicketTypes = new List<TicketType>
            {
                new TicketType { Id = 1, TypeName = "VIP", QuantityAvailable = 50 },
                new TicketType { Id = 2, TypeName = "General", QuantityAvailable = 50 }
            }
        });

        await _repository.CreateAsync(new Event
        {
            Venue = "Venue 1",
            Description = "Event 2",
            Capacity = 100,
            Date = new DateTime(2027,7,7),
            TicketTypes = new List<TicketType>
            {
                new TicketType { Id = 1, TypeName = "VIP", QuantityAvailable = 50 },
                new TicketType { Id = 2, TypeName = "General", QuantityAvailable = 50 }
            }
        });

        // Act
        var allEvents = await _repository.GetAllAsync();

        // Assert
        Assert.That(allEvents.Count(), Is.EqualTo(2));
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnCorrectEvent()
    {
        // Arrange
        var created = await _repository.CreateAsync(new Event
        {
            Venue = "Some Venue",
            Description = "Lookup Event",
            Capacity = 100,
            Date = new DateTime(2027,6,7),
            TicketTypes = new List<TicketType>
            {
                new TicketType { Id = 1, TypeName = "VIP", QuantityAvailable = 50 },
                new TicketType { Id = 2, TypeName = "General", QuantityAvailable = 50 }
            }
        });

        // Act
        var result = await _repository.GetByIdAsync(created.Id);

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result?.Description, Is.EqualTo("Lookup Event"));
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        // Act
        var result = await _repository.GetByIdAsync(999);

        // Assert
        Assert.IsNull(result);
    }

    [Test]
    public async Task UpdateAsync_ShouldUpdateEvent_WhenExists()
    {
        // Arrange
        var created = await _repository.CreateAsync(new Event
        {
            Capacity = 100,
            Description = "Old Description",
            Venue = "Old Venue",
            Date = new DateTime(2026, 1, 1),
            TicketTypes = new List<TicketType> { new TicketType { TypeName= "VIP", QuantityAvailable = 20 } }
        });

        var updated = new Event
        {
            Capacity = 150,
            Description = "New Description",
            Venue = "New Venue",
            Date = new DateTime(2026, 2, 1),
            TicketTypes = new List<TicketType> { new TicketType { TypeName= "VIP", QuantityAvailable = 50 } }
        };

        // Act
        var result = await _repository.UpdateAsync(created.Id, updated);
        var fetched = await _repository.GetByIdAsync(created.Id);

        // Assert
        Assert.IsTrue(result);
        Assert.That(fetched?.Description, Is.EqualTo("New Description"));
        Assert.That(fetched?.Venue, Is.EqualTo("New Venue"));
        Assert.That(fetched?.Date, Is.EqualTo(new DateTime(2026, 2, 1)));
        Assert.That(fetched?.Capacity, Is.EqualTo(150));
    }

    [Test]
    public async Task UpdateAsync_ShouldReturnFalse_WhenEventNotFound()
    {
        // Arrange
        var updated = new Event
        {
            Venue = "Some Venue",
            Description = "Some Event",
            Capacity = 100,
            Date = new DateTime(2027,6,7),
            TicketTypes = new List<TicketType>
            {
                new TicketType { Id = 1, TypeName = "VIP", QuantityAvailable = 50 },
                new TicketType { Id = 2, TypeName = "General", QuantityAvailable = 50 }
            }
        };

        // Act
        var result = await _repository.UpdateAsync(999, updated);

        // Assert
        Assert.IsFalse(result);
    }
}
