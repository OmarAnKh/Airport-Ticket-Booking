namespace Airport_Ticket_Booking.Models.flight;

public interface IFlightRepository
{
    public List<Flight> GetAllData(List<Flight> flights);
    public Flight? GetById(int flightId);
    public void Create(Flight flight);
    public void Update(List<Flight> flights);
    public void Delete(Flight flight);
    public void ImportFlights(string filePath);
}