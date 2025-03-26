namespace Airport_Ticket_Booking.Models.flight;

public interface IBookingManager
{
    bool Book(List<Flight> flights, int flightId,int userId);
    bool Cancel(List<Flight> flights, int flightId);
    bool ModifyClass(List<Flight> flights, int flightId, int classNumber, int userId);
    void DisplayFlights(List<Flight> flights);
}