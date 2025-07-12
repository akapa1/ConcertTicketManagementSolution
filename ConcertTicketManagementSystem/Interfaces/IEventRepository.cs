namespace ConcertTicketManagementSystem.Interfaces;

using ConcertTicketManagementSystem.Models;

public interface IEventRepository
{
    Task<IEnumerable<Event>> GetAllAsync();
    Task<Event?> GetByIdAsync(int id);
    Task<Event> CreateAsync(Event evt);
    Task<bool> UpdateAsync(int id, Event updated);
}

