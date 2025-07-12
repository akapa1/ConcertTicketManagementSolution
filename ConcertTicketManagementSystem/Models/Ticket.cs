namespace ConcertTicketManagementSystem.Models;

public enum TicketStatus
{
    Reserved,
    Purchased,
    Cancelled
}

public class Ticket
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public int TicketTypeId { get; set; }
    public DateTime ReservedAt { get; set; }
    public TicketStatus Status { get; set; }
}