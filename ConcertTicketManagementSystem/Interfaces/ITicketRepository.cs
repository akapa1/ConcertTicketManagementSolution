namespace ConcertTicketManagementSystem.Interfaces;

using ConcertTicketManagementSystem.Models;

public interface ITicketRepository
{
    Task<Ticket?> GetByIdAsync(int ticketId);
    Task<IEnumerable<Ticket>> GetByEventIdAsync(int eventId);
    Task AddAsync(Ticket ticket);
    Task<int> GetReservedCountAsync(int eventId);
    Task<IEnumerable<Ticket>> GetAllAsync();
}
