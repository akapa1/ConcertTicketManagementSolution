namespace ConcertTicketManagementSystem.Repositories;

using ConcertTicketManagementSystem.Interfaces;
using ConcertTicketManagementSystem.Models;

public class TicketRepository : ITicketRepository
{
    private readonly List<Ticket> _tickets = new();
    private int _ticketIdCounter = 1;

    public Task<Ticket?> GetByIdAsync(int ticketId)
    {
        var ticket = _tickets.FirstOrDefault(t => t.Id == ticketId);
        return Task.FromResult(ticket);
    }

    public Task<IEnumerable<Ticket>> GetByEventIdAsync(int eventId)
    {
        var list = _tickets.Where(t => t.EventId == eventId);
        return Task.FromResult(list);
    }

    public Task<IEnumerable<Ticket>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<Ticket>>(_tickets);
    }

    public Task<int> GetReservedCountAsync(int eventId)
    {
        int count = _tickets.Count(t =>
            t.EventId == eventId &&
            (t.Status == TicketStatus.Reserved || t.Status == TicketStatus.Purchased));
        return Task.FromResult(count);
    }

    public Task AddAsync(Ticket ticket)
    {
        ticket.Id = _ticketIdCounter++;
        _tickets.Add(ticket);
        return Task.CompletedTask;
    }
}

