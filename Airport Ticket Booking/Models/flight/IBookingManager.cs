namespace Airport_Ticket_Booking.Models.flight;

public interface IBookingManager
{
    public bool Book(List<Flight> flights, int flightId,int userId);
    public bool Cancel(List<Flight> flights, int flightId);
    public bool ModifyClass(List<Flight> flights, int flightId, int classNumber, int userId);
    public void DisplayFlights(List<Flight> flights);
}