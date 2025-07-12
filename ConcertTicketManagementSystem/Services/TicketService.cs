namespace ConcertTicketManagementSystem.Services;

using ConcertTicketManagementSystem.Interfaces;
using ConcertTicketManagementSystem.Models;

public class TicketService : ITicketService
{
    private readonly IEventService _eventService;
    private readonly ITicketRepository _ticketRepository;
    private readonly TimeSpan _reservationWindow = TimeSpan.FromMinutes(10);

    public TicketService(IEventService eventService, ITicketRepository ticketRepository)
    {
        _eventService = eventService;
        _ticketRepository = ticketRepository;
    }

    public async Task<Ticket?> ReserveTicketAsync(int eventId, int ticketTypeId)
    {
        var evt = await _eventService.GetByIdAsync(eventId);
        if (evt == null) return null;

        // Remove expired reservations
        var allTickets = await _ticketRepository.GetAllAsync();
        var now = DateTime.UtcNow;

        foreach (var tkt in allTickets)
        {
            if (tkt.Status == TicketStatus.Reserved && now - tkt.ReservedAt > _reservationWindow)
            {
                tkt.Status = TicketStatus.Cancelled;
            }
        }

        // Get total reserved tickets for this event
        var tickets = await _ticketRepository.GetByEventIdAsync(eventId);
        int reservedCount = tickets.Count(t =>
            t.Status == TicketStatus.Reserved || t.Status == TicketStatus.Purchased);

        if (reservedCount >= evt.Capacity)
            return null;

        var type = evt.TicketTypes.FirstOrDefault(t => t.Id == ticketTypeId);
        if (type == null || type.QuantityAvailable <= 0) return null;

        type.QuantityAvailable--;

        var ticket = new Ticket
        {
            EventId = eventId,
            TicketTypeId = ticketTypeId,
            ReservedAt = DateTime.UtcNow,
            Status = TicketStatus.Reserved
        };

        await _ticketRepository.AddAsync(ticket);
        return ticket;
    }

    public async Task<bool> PurchaseTicketAsync(int ticketId)
    {
        var ticket = await _ticketRepository.GetByIdAsync(ticketId);
        
        // Check if status is Reserved
        if (ticket == null || ticket.Status != TicketStatus.Reserved)
        {
            return false;
        }

        // Check if reservation is still valid
        if (DateTime.Now - ticket.ReservedAt > _reservationWindow)
        {
            ticket.Status = TicketStatus.Cancelled;
            return false;
        }

        // Here we assume payment system call succeeds
        // await method may come here while calling Purchasing system and also the Price can be set here???
        ticket.Status = TicketStatus.Purchased;
        return true;
    }

    public async Task<bool> CancelReservationAsync(int ticketId)
    {
        var ticket = await _ticketRepository.GetByIdAsync(ticketId);
        if (ticket == null || ticket.Status != TicketStatus.Purchased)
        {
            return false;
        }

        var evt = await _eventService.GetByIdAsync(ticket.EventId);
        var type = evt?.TicketTypes.FirstOrDefault(t => t.Id == ticket.TicketTypeId);
        if (type != null)
        {
            type.QuantityAvailable++;
        }

        ticket.Status = TicketStatus.Cancelled;
        return true;
    }

    public async Task<IEnumerable<Ticket>> GetTicketsByEventID(int eventId)
    {
        return await _ticketRepository.GetByEventIdAsync(eventId);
    }
}