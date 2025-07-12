namespace ConcertTicketManagementSystem.Interfaces;

using ConcertTicketManagementSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IEventService
{
    Task<IEnumerable<Event>> GetAllAsync();
    Task<Event?> GetByIdAsync(int id);
    Task<Event> CreateAsync(Event evt);
    Task<bool> UpdateAsync(int id, Event updated);
}
