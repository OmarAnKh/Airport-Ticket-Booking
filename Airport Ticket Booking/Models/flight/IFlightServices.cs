namespace Airport_Ticket_Booking.Models.flight;

public interface IFlightServices
{
    void SearchFlights(string? departureCountry = null,
        string? destinationCountry = null, DateTime? departureDate = null,
        string? departureAirport = null, string? arrivalAirport = null,
        string? flightClass = null, decimal? maxPrice = null);

    void FilterFlights(int? flightId = null, decimal? price = null,
        string? departureCountry = null,
        string? destinationCountry = null, DateTime? departureDate = null, string? departureAirport = null,
        string? arrivalAirport = null, int? passenger = null, int flightClass = 0);

    bool BookFlight(int flightId, int userId);

    public bool CancelFlight(int flightId);

    public void ShowMyFlights(int userId);
    public bool ModifyFlight(int flightId, int classNumber, int userId);

    public List<Flight> GetAllFlights();
    public List<string>? ImportFlightsFromCsv(string filePath);
    public void DisplayFlights(List<Flight> flights);
}