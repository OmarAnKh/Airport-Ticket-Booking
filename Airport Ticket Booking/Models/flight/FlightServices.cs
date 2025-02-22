namespace Airport_Ticket_Booking.Models.flight;

public class FlightServices : IFlightServices
{
    private List<Flight> _flights = [];
    private readonly IFlightSearchServices _flightSearchServices;
    private readonly IFlightRepository? _repository;
    private readonly IBookingManager _bookingManager;

    public FlightServices(string flightFilePath)
    {
        _flightSearchServices = new FlightSearchServices();
        _repository = FlightRepository.GetInstance(flightFilePath);
        _repository?.GetAllData(_flights);
        _bookingManager = new BookingManager();
    }

    public List<Flight> GetAllFlights()
    {
        return _flights;
    }

    public void SearchFlights(string? departureCountry = null,
        string? destinationCountry = null, DateTime? departureDate = null,
        string? departureAirport = null, string? arrivalAirport = null, string? flightClass = null,
        decimal? maxPrice = null)
    {
        var domFlight = _flightSearchServices.SearchFlights(_flights, departureCountry, destinationCountry,
            departureDate,
            departureAirport, arrivalAirport, flightClass, maxPrice);
        _bookingManager.DisplayFlights(domFlight);
    }

    public bool BookFlight(int flightId, int userId)
    {
        var isBooked = _bookingManager.Book(_flights, flightId, userId);
        if (isBooked)
        {
            _repository?.Update(_flights);
            return true;
        }

        return false;
    }

    public bool ModifyFlight(int flightId, int classNumber, int userId)
    {
        bool isModified = _bookingManager.ModifyClass(_flights, flightId, classNumber, userId);
        if (isModified)
        {
            _repository?.Update(_flights);
            return true;
        }

        return false;
    }


    public bool CancelFlight(int flightId)
    {
        var isBooked = _bookingManager.Cancel(_flights, flightId);
        if (isBooked)
        {
            _repository?.Update(_flights);
            return true;
        }

        return false;
    }

    public void ShowMyFlights(int userId)
    {
        var myFlights = _flights.Where(flight => flight.PassengerId == userId).ToList();
        _bookingManager.DisplayFlights(myFlights);
    }

    public List<string> ImportFlightsFromCsv(string filePath)
    {
        Dictionary<string, object> imports = _repository.ImportFlights(filePath);
        _flights = _flights.Concat(imports["Flights"] as List<Flight>).ToList();
        _repository.Update(_flights);
        return imports["Errors"] as List<string>;
    }
}