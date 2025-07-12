namespace ConcertTicketManagementSystem.Controllers;

using ConcertTicketManagementSystem.Interfaces;
using ConcertTicketManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IEventService _service;

    public EventsController(IEventService service)
    {
        _service = service;
    }

    /// <summary>
    /// Retrieves a list of all available events.
    /// </summary>
    /// <returns>A list of all events.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Event>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Event>>> GetAllAsync()
    {
        return Ok(await _service.GetAllAsync());
    }

    /// <summary>
    /// Retrieves a specific event by its ID.
    /// </summary>
    /// <param name="eventId">The ID of the event to retrieve.</param>
    /// <returns>The event with the specified ID, or 404 if not found.</returns>
    [HttpGet("{eventId}")]
    [ProducesResponseType(typeof(Event), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Event>> GetByIdAsync(int eventId)
    {
        var evt = await _service.GetByIdAsync(eventId);
        return evt == null ? NotFound() : Ok(evt);
    }

    /// <summary>
    /// Creates a new event.
    /// </summary>
    /// <param name="eventBody">The event data to create.</param>
    /// <returns>Returns the created event; otherwise, returns 400 Bad Request.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Event), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Event>> CreateAsync(Event eventBody)
    {
        var created = await _service.CreateAsync(eventBody);
        return created;
    }

    /// <summary>
    /// Updates an existing event.
    /// </summary>
    /// <param name="eventId">The ID of the event to update.</param>
    /// <param name="updated">The updated event data.</param>
    /// <returns>No content if successful; 404 Not Found if the event does not exist.</returns>
    [HttpPut("{eventId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAsync(int eventId, Event updated)
    {
        if (!await _service.UpdateAsync(eventId, updated)) return NotFound();
        return NoContent();
    }
}
