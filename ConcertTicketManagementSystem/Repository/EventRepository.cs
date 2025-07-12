namespace ConcertTicketManagementSystem.Repositories;

using ConcertTicketManagementSystem.Interfaces;
using ConcertTicketManagementSystem.Models;

public class EventRepository : IEventRepository
{
    private readonly List<Event> _events = new();
    private int _eventIdCounter = 1;

    public Task<IEnumerable<Event>> GetAllAsync()
    {
        return Task.FromResult(_events.AsEnumerable());
    }

    public Task<Event?> GetByIdAsync(int id)
    {
        var evt = _events.FirstOrDefault(e => e.Id == id);
        return Task.FromResult(evt);
    }

    public Task<Event> CreateAsync(Event evt)
    {
        int totalTicketQuantity = evt.TicketTypes.Sum(ticketType => ticketType.QuantityAvailable);
        if (totalTicketQuantity > evt.Capacity)
        {
            throw new BadHttpRequestException("Total ticket quantity exceeds event capacity.");
        }

        evt.Id = _eventIdCounter++;
        _events.Add(evt);
        return Task.FromResult(evt);
    }

    public Task<bool> UpdateAsync(int id, Event updated)
    {
        var existing = _events.FirstOrDefault(e => e.Id == id);
        if (existing == null) return Task.FromResult(false);
        
        existing.Date = updated.Date;
        existing.Venue = updated.Venue;
        existing.Description = updated.Description;
        existing.Capacity = updated.Capacity;
        existing.TicketTypes = updated.TicketTypes;

        return Task.FromResult(true);
    }
}

