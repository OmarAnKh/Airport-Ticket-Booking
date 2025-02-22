namespace Airport_Ticket_Booking.Models.flight;

public interface IFlightRepository
{
    public List<Flight> GetAllData(List<Flight> flights);
    public void Update(List<Flight> flights);
    public Dictionary<string, object> ImportFlights(string filePath);
}