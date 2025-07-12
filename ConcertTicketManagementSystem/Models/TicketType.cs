namespace ConcertTicketManagementSystem.Models;

public class TicketType
{
    public int Id { get; set; }
    public required string TypeName { get; set; }
    public decimal Price { get; set; }
    public int QuantityAvailable { get; set; }
}