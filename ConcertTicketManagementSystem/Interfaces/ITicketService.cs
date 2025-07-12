namespace ConcertTicketManagementSystem.Interfaces;

using System.Threading.Tasks;
using ConcertTicketManagementSystem.Models;

public interface ITicketService
{
    Task<Ticket?> ReserveTicketAsync(int eventId, int ticketTypeId);
    Task<bool> PurchaseTicketAsync(int ticketId);
    Task<bool> CancelReservationAsync(int ticketId);
    Task<IEnumerable<Ticket>> GetTicketsByEventID(int eventId);
}