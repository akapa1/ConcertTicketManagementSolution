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
public class TicketsControllerTests
{
    private Mock<ITicketService> _mockTicketService;
    private TicketsController _ticketsController;

    [SetUp]
    public void SetUp()
    {
        _mockTicketService = new Mock<ITicketService>();
        _ticketsController = new TicketsController(_mockTicketService.Object);
    }

    [Test]
    public async Task ReserveAsync_ReturnsOk_WhenTicketIsReserved()
    {
        // Arrange
        var ticket = new Ticket { Id = 1, EventId = 10, TicketTypeId = 2 };
        _mockTicketService.Setup(s => s.ReserveTicketAsync(10, 2))
                    .ReturnsAsync(ticket);

        // Act
        var result = await _ticketsController.ReserveAsync(10, 2);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result.Result);
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult?.Value, Is.EqualTo(ticket));
    }

    [Test]
    public async Task ReserveAsync_ReturnsBadRequest_WhenReservationFails()
    {
        // Arrange
        _mockTicketService.Setup(s => s.ReserveTicketAsync(10, 2))
                    .ReturnsAsync((Ticket)null);

        // Act
        var result = await _ticketsController.ReserveAsync(10, 2);

        // Assert
        Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
    }

    [Test]
    public async Task PurchaseAsync_ReturnsOk_WhenSuccessful()
    {
        // Arrange
        _mockTicketService.Setup(s => s.PurchaseTicketAsync(5))
                    .ReturnsAsync(true);

        // Act
        var result = await _ticketsController.PurchaseAsync(5);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
    }

    [Test]
    public async Task PurchaseAsync_ReturnsBadRequest_WhenFails()
    {
        // Arrange
        _mockTicketService.Setup(s => s.PurchaseTicketAsync(5))
                    .ReturnsAsync(false);

        // Act
        var result = await _ticketsController.PurchaseAsync(5);

        // Assert
        Assert.IsInstanceOf<BadRequestObjectResult>(result);
    }

    [Test]
    public async Task CancelAsync_ReturnsOk_WhenCancelled()
    {
        // Arrange
        _mockTicketService.Setup(s => s.CancelReservationAsync(7))
                    .ReturnsAsync(true);
       
        // Act
        var result = await _ticketsController.CancelAsync(7);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
    }

    [Test]
    public async Task CancelAsync_ReturnsBadRequest_WhenCancellationFails()
    {
        // Arrange
        _mockTicketService.Setup(s => s.CancelReservationAsync(7))
                    .ReturnsAsync(false);

        // Act
        var result = await _ticketsController.CancelAsync(7);

        // Assert
        Assert.IsInstanceOf<BadRequestObjectResult>(result);
    }

    [Test]
    public async Task GetByEventAsync_ReturnsListOfTickets()
    {
        // Arrange
        var tickets = new List<Ticket>
        {
            new Ticket { Id = 1, EventId = 5 },
            new Ticket { Id = 2, EventId = 5 }
        };

        // Act
        _mockTicketService.Setup(s => s.GetTicketsByEventID(5))
                    .ReturnsAsync(tickets);

        var result = await _ticketsController.GetByEventAsync(5);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result.Result);
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult?.Value, Is.EqualTo(tickets));
    }
}

