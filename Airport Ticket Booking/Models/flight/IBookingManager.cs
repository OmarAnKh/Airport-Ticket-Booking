namespace Airport_Ticket_Booking.Models.flight;

public interface IBookingManager
{
    public bool Book(List<Flight> flights, int flightId);
    public bool Cancel();
    public bool Modify();
}