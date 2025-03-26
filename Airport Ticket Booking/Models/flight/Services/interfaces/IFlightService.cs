namespace Airport_Ticket_Booking.Models.flight.Services.interfaces;

public interface IFlightService
{
    List<Flight> SearchFlights(FlightSearchCriteria searchCriteria);

    List<Flight> FilterFlights(FlightFilterCriteria filterCriteria);

    bool BookFlight(int flightId, int userId);

    bool CancelFlight(int flightId);

    bool ShowMyFlights(int userId); 
    bool ModifyFlight(int flightId, int classNumber, int userId);

    List<Flight> GetAllFlights();
    Task<List<string>?> ImportFlightsFromCsvAsync(string filePath);
    void DisplayFlights(List<Flight> flights);
}