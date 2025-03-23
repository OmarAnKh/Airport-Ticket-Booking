namespace Airport_Ticket_Booking.Models.flight.Repositories.Interfaces;

public interface IFlightRepository
{
    List<Flight> GetAllData();
    Task UpdateAsync(List<Flight> flights);
    Task<Dictionary<string, object?>> ImportFlightsAsync(string importFilePath);
    
}