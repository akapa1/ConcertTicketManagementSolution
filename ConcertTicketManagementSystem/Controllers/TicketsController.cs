namespace ConcertTicketManagementSystem.Controllers;

using ConcertTicketManagementSystem.Interfaces;
using ConcertTicketManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly ITicketService _service;

    public TicketsController(ITicketService service)
    {
        _service = service;
    }

    /// <summary>
    /// Reserves a ticket for a given event and ticket type.
    /// </summary>
    /// <param name="eventId">The ID of the event to reserve a ticket for.</param>
    /// <param name="ticketTypeId">The ID of the ticket type to reserve.</param>
    /// <returns>
    /// Returns the reserved ticket if successful; otherwise, returns a 400 Bad Request response.
    /// </returns>
    [HttpPost("reserve")]
    [ProducesResponseType(typeof(Ticket), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Ticket>> ReserveAsync(int eventId, int ticketTypeId)
    {
        var ticket = await _service.ReserveTicketAsync(eventId, ticketTypeId);
        return ticket == null ? BadRequest("Unable to reserve") : Ok(ticket);
    }

    /// <summary>
    /// Purchases a reserved ticket by ticket ID.
    /// </summary>
    /// <param name="ticketId">The ID of the ticket to purchase.</param>
    /// <returns>
    /// Returns 200 OK if the ticket was successfully purchased; otherwise, returns 400 Bad Request.
    /// </returns>
    [HttpPost("purchase/{ticketId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PurchaseAsync(int ticketId)
    {
        return await _service.PurchaseTicketAsync(ticketId) ? Ok("Purchased") : BadRequest("Cannot purchase");
    }

    /// <summary>
    /// Cancels a reserved ticket by ticket ID.
    /// </summary>
    /// <param name="ticketId">The ID of the ticket to cancel.</param>
    /// <returns>
    /// Returns 200 OK if the reservation was successfully cancelled; otherwise, returns 400 Bad Request.
    /// </returns>
    [HttpPost("cancel/{ticketId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CancelAsync(int ticketId)
    {
        return await _service.CancelReservationAsync(ticketId) ? Ok("Cancelled") : BadRequest("Cannot cancel");
    }

    /// <summary>
    /// Retrieves all tickets associated with a specific event.
    /// </summary>
    /// <param name="eventId">The ID of the event.</param>
    /// <returns>
    /// Returns a list of tickets for the specified event.
    /// </returns>
    [HttpGet("event/{eventId}")]
    [ProducesResponseType(typeof(IEnumerable<Ticket>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Ticket>>> GetByEventAsync(int eventId)
    {
        return Ok(await _service.GetTicketsByEventID(eventId));
    }
}