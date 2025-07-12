namespace ConcertTicketManagementSystem.Tests.Repositories;

using ConcertTicketManagementSystem.Models;
using ConcertTicketManagementSystem.Repositories;

[TestFixture]
public class TicketRepositoryTests
{
    private TicketRepository _repository;

    [SetUp]
    public void Setup()
    {
        _repository = new TicketRepository();
    }

    [Test]
    public async Task AddAsync_ShouldAddTicketAndAssignId()
    {
        // Arrange
        var ticket = new Ticket
        {
            EventId = 1,
            TicketTypeId = 1,
            Status = TicketStatus.Reserved,
            ReservedAt = DateTime.UtcNow
        };

        // Act
        await _repository.AddAsync(ticket);
        var allTickets = await _repository.GetAllAsync();

        // Assert
        Assert.That(ticket.Id, Is.EqualTo(1));
        Assert.That(allTickets.Count(), Is.EqualTo(1));
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnCorrectTicket()
    {
        // Arrange
        var ticket = new Ticket
        {
            EventId = 2,
            TicketTypeId = 2,
            Status = TicketStatus.Reserved,
            ReservedAt = DateTime.UtcNow
        };
        await _repository.AddAsync(ticket);

        // Act
        var result = await _repository.GetByIdAsync(ticket.Id);

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result?.EventId, Is.EqualTo(2));
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnNull_WhenTicketNotFound()
    {
        // Act
        var result = await _repository.GetByIdAsync(999);

        // Assert
        Assert.IsNull(result);
    }

    [Test]
    public async Task GetByEventIdAsync_ShouldReturnMatchingTickets()
    {
        // Arrange
        var ticket1 = new Ticket { EventId = 5, TicketTypeId = 1, Status = TicketStatus.Reserved };
        var ticket2 = new Ticket { EventId = 5, TicketTypeId = 2, Status = TicketStatus.Purchased };
        var ticket3 = new Ticket { EventId = 6, TicketTypeId = 1, Status = TicketStatus.Reserved };

        await _repository.AddAsync(ticket1);
        await _repository.AddAsync(ticket2);
        await _repository.AddAsync(ticket3);

        // Act
        var result = await _repository.GetByEventIdAsync(5);

        // Assert
        Assert.That(result.Count(), Is.EqualTo(2));
    }

    [Test]
    public async Task GetAllAsync_ShouldReturnAllTickets()
    {
        // Arrange
        await _repository.AddAsync(new Ticket { EventId = 1, Status = TicketStatus.Reserved });
        await _repository.AddAsync(new Ticket { EventId = 2, Status = TicketStatus.Cancelled });

        // Act
        var all = await _repository.GetAllAsync();

        // Assert
        Assert.That(all.Count(), Is.EqualTo(2));
    }

    [Test]
    public async Task GetReservedCountAsync_ShouldCountOnlyReservedAndPurchased()
    {
        // Arrange
        await _repository.AddAsync(new Ticket { EventId = 3, Status = TicketStatus.Reserved });
        await _repository.AddAsync(new Ticket { EventId = 3, Status = TicketStatus.Purchased });
        await _repository.AddAsync(new Ticket { EventId = 3, Status = TicketStatus.Cancelled });

        // Act
        var count = await _repository.GetReservedCountAsync(3);

        // Assert
        Assert.That(count, Is.EqualTo(2));
    }
}
