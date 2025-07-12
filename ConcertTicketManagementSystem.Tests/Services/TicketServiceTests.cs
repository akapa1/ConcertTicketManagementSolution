namespace ConcertTicketManagementSystem.Tests.Services;

using System.Reflection;
using ConcertTicketManagementSystem.Interfaces;
using ConcertTicketManagementSystem.Models;
using ConcertTicketManagementSystem.Services;
using Moq;

[TestFixture]
public class TicketServiceTests
{
    private Mock<IEventService> _mockEventService;
    private Mock<ITicketRepository> _mockTicketRepository;
    private TicketService _ticketService;
    
    [SetUp]
    public void Setup()
    {
        _mockEventService = new Mock<IEventService>();
        _mockTicketRepository = new Mock<ITicketRepository>();
        _ticketService = new TicketService(_mockEventService.Object, _mockTicketRepository.Object);
    }

    private Event CreateEvent(int eventId, int capacity, string venue, string description, DateTime dateTime, int ticketTypeId, int quantityAvailable, string typeName)
    {
        return new Event
        {
            Id = eventId,
            Capacity = capacity,
            Venue = venue,
            Description = description,
            Date = dateTime,
            TicketTypes = new List<TicketType>
            {
                new TicketType { Id = ticketTypeId, TypeName = typeName, QuantityAvailable = quantityAvailable }
            }
        };
    }

    public async Task ReserveTicketAsync_ReturnsTicket_WhenSuccessful()
    {
        // Arrange
        var evt = CreateEvent(1, 100, "Venue A", "Katy Perry Concert", new DateTime(2027,5,5), 10, 5, "VIP");
        _mockEventService.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(evt);
        _mockTicketRepository.Setup(x => x.AddAsync(It.IsAny<Ticket>())).Returns(Task.CompletedTask);

        // Act
        var ticket = await _ticketService.ReserveTicketAsync(1, 10);

        // Assert
        Assert.IsNotNull(ticket);
        Assert.That(ticket!.Status, Is.EqualTo(TicketStatus.Reserved));
        _mockTicketRepository.Verify(x => x.AddAsync(It.IsAny<Ticket>()), Times.Once);
    }

    [Test]
    public async Task ReserveTicketAsync_ReturnsNull_WhenEventDoesNotExist()
    {
        // Arrange
        _mockEventService.Setup(x => x.GetByIdAsync(99)).ReturnsAsync((Event?)null);

        // Act
        var ticket = await _ticketService.ReserveTicketAsync(99, 1);

        // Assert
        Assert.IsNull(ticket);
    }

    [Test]
    public async Task ReserveTicketAsync_ReturnsNull_WhenCapacityIsExceeded()
    {
        // Arrange
        var evt = CreateEvent(1, 1, "Venue A", "Katy Perry Concert", new DateTime(2027,5,5), 10, 5, "VIP");
        var existingTickets = new List<Ticket>
        {
            new Ticket { EventId = 1, Status = TicketStatus.Reserved }
        };
        _mockEventService.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(evt);
        _mockTicketRepository.Setup(x => x.GetByEventIdAsync(1)).ReturnsAsync(existingTickets);

        // Act
        var result = await _ticketService.ReserveTicketAsync(1, 10);

        // Assert
        Assert.IsNull(result);
    }

    [Test]
    public async Task PurchaseTicketAsync_ReturnsTrue_WhenReserved()
    {
        // Arrange
        var ticket = new Ticket { Id = 1, Status = TicketStatus.Reserved, ReservedAt = DateTime.Now.Subtract(TimeSpan.FromMinutes(5)) };
        _mockTicketRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(ticket);

        // Act
        var result = await _ticketService.PurchaseTicketAsync(1);

        // Assert
        Assert.IsTrue(result);
        Assert.That(ticket.Status, Is.EqualTo(TicketStatus.Purchased));
    }

    [Test]
    public async Task PurchaseTicketAsync_ReturnsFalse_WhenTicketNotFound()
    {
        // Arrange
        _mockTicketRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync((Ticket?)null);

        // Act
        var result = await _ticketService.PurchaseTicketAsync(1);

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public async Task PurchaseTicketAsync_ReturnsFalse_WhenTicketNotReserved()
    {
        // Arrange
        var ticket = new Ticket { Id = 1, Status = TicketStatus.Purchased };
        _mockTicketRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(ticket);

        // Act
        var result = await _ticketService.PurchaseTicketAsync(1);

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public async Task CancelReservationAsync_ReturnsTrue_WhenSuccess()
    {
        // Arrange
        var ticket = new Ticket { Id = 1, Status = TicketStatus.Purchased, EventId = 1, TicketTypeId = 10 };
        var evt = CreateEvent(1, 100, "Venue A", "Katy Perry Concert", new DateTime(2027,5,5), 10, 5, "General");

        _mockTicketRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(ticket);
        _mockEventService.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(evt);

        // Act
        var result = await _ticketService.CancelReservationAsync(1);

        // Assert
        Assert.IsTrue(result);
        Assert.That(ticket.Status, Is.EqualTo(TicketStatus.Cancelled));
        Assert.That(evt.TicketTypes.First().QuantityAvailable, Is.EqualTo(6));
    }

    [Test]
    public async Task CancelReservationAsync_ReturnsFalse_WhenTicketNotFound()
    {
        // Arrange
        _mockTicketRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync((Ticket?)null);

        // Act
        var result = await _ticketService.CancelReservationAsync(1);

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public async Task CancelReservationAsync_ReturnsFalse_WhenStatusIsInvalid()
    {
        // Arrange
        var ticket = new Ticket { Id = 1, Status = TicketStatus.Reserved };
        _mockTicketRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(ticket);

        // Act
        var result = await _ticketService.CancelReservationAsync(1);

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public async Task GetAvailableTicketsAsync_ReturnsFilteredTickets()
    {
        // Arrange
        var tickets = new List<Ticket>
        {
            new Ticket { Id = 1, EventId = 1 },
            new Ticket { Id = 2, EventId = 1 },
            new Ticket { Id = 3, EventId = 2 }
        };

        _mockTicketRepository.Setup(x => x.GetByEventIdAsync(1)).ReturnsAsync(tickets.Where(t => t.EventId == 1));

        // Act
        var result = await _ticketService.GetTicketsByEventID(1);

        // Assert
        Assert.That(result.Count(), Is.EqualTo(2));
    }
}
