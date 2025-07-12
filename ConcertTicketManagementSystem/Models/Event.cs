namespace ConcertTicketManagementSystem.Models;

public class Event
{
    public int Id { get; set; }
    public required string Venue { get; set; }
    public required DateTime Date { get; set; }
    public required string Description { get; set; }
    public int Capacity { get; set; }
    public List<TicketType> TicketTypes { get; set; } = new();
}