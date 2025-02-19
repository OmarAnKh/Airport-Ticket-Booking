namespace Airport_Ticket_Booking.Models.flight;

public class Flights
{
    private static Flights? _instance;
    private readonly string _filePath;
    private readonly List<Flight> _flights;
    private static readonly Lock Lock = new();
    private readonly IFlightRepository _repository;
    private readonly IFlightSearchServices _flightSearchServices;

    private Flights(string filePath)
    {
        _filePath = filePath;
        _flightSearchServices = new FlightSearchServices();
        _repository = new FlightRepository(filePath);
        _flights = _repository.GetAllData();
    }

    public static Flights GetInstance(string filePath)
    {
        lock (Lock)
        {
            _instance ??= new Flights(filePath);
        }

        return _instance;
    }

    public void SearchFlights(string? departureCountry = null,
        string? destinationCountry = null, DateTime? departureDate = null,
        string? departureAirport = null, string? arrivalAirport = null, string? flightClass = null,
        decimal? maxPrice = null)
    {
        var domFlight = _flightSearchServices.SearchFlights(_flights, departureCountry, destinationCountry,
            departureDate,
            departureAirport, arrivalAirport, flightClass, maxPrice);
        Console.Read();
    }
}