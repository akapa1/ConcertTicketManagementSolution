namespace ConcertTicketManagementSystem.Services;

using ConcertTicketManagementSystem.Interfaces;
using ConcertTicketManagementSystem.Models;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;

        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

    public async Task<IEnumerable<Event>> GetAllAsync()
    {
        return await _eventRepository.GetAllAsync();
    }

    public async Task<Event?> GetByIdAsync(int id) {
       return await _eventRepository.GetByIdAsync(id);
    } 

    public async Task<Event> CreateAsync(Event evt)
    {
        return await _eventRepository.CreateAsync(evt);
    }

    public async Task<bool> UpdateAsync(int id, Event updated)
    {
        return await _eventRepository.UpdateAsync(id, updated);
    }
}