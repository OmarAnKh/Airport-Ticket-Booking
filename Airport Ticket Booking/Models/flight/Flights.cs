namespace Airport_Ticket_Booking.Models.flight;

public class Flights
{
    private static Flights? _instance;
    private readonly string _filePath;
    private readonly List<Flight> _flights = [];
    private static readonly Lock Lock = new();
    private readonly IFlightSearchServices _flightSearchServices;
    private readonly IFlightRepository _repository;
    private readonly IBookingManager _bookingManager;

    private Flights(string filePath)
    {
        _filePath = filePath;
        _flightSearchServices = new FlightSearchServices();
        _repository = new FlightRepository(filePath);
        _repository.GetAllData(_flights);
        _bookingManager = new BookingManager();
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

    public void BookFlight(int flightId)
    {
        var result = _bookingManager.Book(_flights, flightId);
        if (result)
        {
            _repository.Update(_flights, _filePath);
        }
    }
}