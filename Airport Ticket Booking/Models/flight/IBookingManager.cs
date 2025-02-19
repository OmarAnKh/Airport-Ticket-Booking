namespace Airport_Ticket_Booking.Models.flight;

public interface IBookingManager
{
    public bool Book(List<Flight> flights, int flightId);
    public bool Cancel(List<Flight> flights, int flightId);
    public bool Modify();
    public void DisplayFlights(List<Flight> flights);
}