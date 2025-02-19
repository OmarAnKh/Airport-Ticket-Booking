using Airport_Ticket_Booking.Models.user;

namespace Airport_Ticket_Booking.Models.flight;

public class Flights
{
    private static Flights? _instance;
    private readonly List<Flight> _flights = [];
    private static readonly Lock Lock = new();
    private  User? _user;
    private readonly IFlightSearchServices _flightSearchServices;
    private readonly IFlightRepository _repository;
    private readonly IBookingManager _bookingManager;

    private Flights(string flightFilePath, string userFilePath)
    {
        _flightSearchServices = new FlightSearchServices();
        _repository = new FlightRepository(flightFilePath);
        _repository.GetAllData(_flights);
        _bookingManager = new BookingManager();
        _user = null;
    }

    public static Flights GetInstance(string flightFilePath, string userFilePath)
    {
        lock (Lock)
        {
            _instance ??= new Flights(flightFilePath, userFilePath);
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
        var isBooked = _bookingManager.Book(_flights, flightId);
        if (isBooked)
        {
            _repository.Update(_flights);
        }
    }

    public void CancelFlight(int flightId)
    {
        var isBooked = _bookingManager.Cancel(_flights, flightId);
        if (isBooked)
        {
            _repository.Update(_flights);
        }
    }

    public void ShowMyFlights()
    {
        var myFlights = _flights.Where(flight => flight.PassengerId == 123).ToList();
        _bookingManager.DisplayFlights(myFlights);
    }
    
}