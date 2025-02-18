namespace Airport_Ticket_Booking.Models.flight;

public interface IFlightRepository
{
    public List<Flight> GetAllData();
    public Flight? GetById(int flightId);
    public void Create(Flight flight);
    public void Update(Flight flight);
    public void Delete(Flight flight);
    public void ImportFlights(string filePath);
}