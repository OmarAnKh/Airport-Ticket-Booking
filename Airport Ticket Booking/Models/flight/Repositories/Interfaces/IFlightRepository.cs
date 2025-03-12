namespace Airport_Ticket_Booking.Models.flight.Repositories.Interfaces;

public interface IFlightRepository
{
    public List<Flight> GetAllData();
    public void Update(List<Flight> flights);
    public Dictionary<string, object?> ImportFlights(string filePath);
}