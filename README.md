# Concert Ticket Management System
A simple backend system for managing events and ticket reservations, built with ASP.NET Core, C# and tested with NUnit. This application uses an in-memory repository pattern.

# Domain Features from the exercise guidelines that were implemented
# Events
- Create/update concert events
    - Developed endpoints to allow creating new events and updating existing ones.
    - Added validation logic to ensure the sum of ticket quantities doesn’t exceed the event’s total capacity.
- Set ticket types and pricing
    - Each event supports multiple ticket types (e.g., VIP, General).
    - Implemented models to store ticket type details including QuantityAvailable and Price.
- Manage available capacity
    - Prevents event creation if total available tickets exceed the specified capacity.
    - Built logic inside the repository to enforce this rule at data level.
- Basic event details (date, venue, description)
    - Included properties for setting event Date, Venue and Description as required params. (acts as the validation for the input)

# Tickets
- Reserve tickets for a time window
    - Implemented logic to allow users to reserve tickets (e.g., chose 10 minutes).
    - Reserved tickets are marked with a Reserved status and a ReservedAt timestamp.
    - Check everytime when reserving a ticket if any of the reserved tickets are expired and mark them as cancelled.
    - System prevents overbooking by tracking how many tickets are currently reserved or purchased.
- Purchase tickets (Can assume there is already Payment Processing System in place which you can leverage)
    - Users can purchase a ticket that is already reserved.
    - Only tickets in Reserved status and not expired are allowed to be purchased.
    - On purchase, the ticket status changes to Purchased.
    - Assumes integration with an external Payment Processing System is success.
- Cancel reservations
    - Users can cancel a purchased ticket.
    - On cancellation, the ticket status changes to Cancelled.
    - The corresponding ticket type's QuantityAvailable is incremented again to allow others to reserve it.
- View ticket availability
    - Implemented an endpoint to list all tickets for a given event.
    - Includes status of each ticket (Reserved, Purchased, Cancelled).
    - Can be extended to count how many tickets are still available based on the status.

# How to run the app
- Build : dotnet build
- Test : dotnet test ConcertTicketManagementSystem.Tests
- Run : dotnet run --project ConcertTicketManagementSystem

# Future enhancements I can think of currently
- Implement role based access for the API (e.g., Admins/Users)
- Containarize
- Implement with actual database.
- Add cache layer to decrease the latency