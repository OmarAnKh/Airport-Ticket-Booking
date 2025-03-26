using Airport_Ticket_Booking.Models.flight.Repositories.Interfaces;
using Airport_Ticket_Booking.Models.flight.Services.interfaces;

namespace Airport_Ticket_Booking.Models.flight.Services;

public class FlightService : IFlightService
{
    private List<Flight> _flights;
    private readonly IFlightSearchService _flightSearchService;
    private readonly IFlightFilterService _flightFilterService;
    private readonly IFlightRepository _repository;
    private readonly IBookingManager _bookingManager;

    public FlightService(IFlightSearchService flightSearchService, IFlightRepository repository,
        IBookingManager bookingManager, IFlightFilterService flightFilterService)
    {
        _flightSearchService = flightSearchService;
        _repository = repository;
        _bookingManager = bookingManager;
        _flightFilterService = flightFilterService;
        _flights = _repository.GetAllData();
    }

    public List<Flight> GetAllFlights()
    {
        return _flights;
    }

    public List<Flight> SearchFlights(FlightSearchCriteria searchCriteria)
    {
        var domFlight = _flightSearchService.SearchFlights(_flights, searchCriteria);
        return domFlight;
    }

    public List<Flight> FilterFlights(FlightFilterCriteria filterCriteria)
    {
        var filteredFlights = _flightFilterService.FilterFlights(_flights,filterCriteria);
        return filteredFlights;
    }

    public void DisplayFlights(List<Flight> flights)
    {
        _bookingManager.DisplayFlights(flights);
    }

    public bool BookFlight(int flightId, int userId)
    {
        var isBooked = _bookingManager.Book(_flights, flightId, userId);
        if (isBooked)
        {
            _repository.UpdateAsync(_flights);
            return true;
        }

        return false;
    }

    public bool ModifyFlight(int flightId, int classNumber, int userId)
    {
        bool isModified = _bookingManager.ModifyClass(_flights, flightId, classNumber, userId);
        if (isModified)
        {
            _repository.UpdateAsync(_flights);
            return true;
        }

        return false;
    }


    public bool CancelFlight(int flightId)
    {
        var isBooked = _bookingManager.Cancel(_flights, flightId);
        if (isBooked)
        {
            _repository.UpdateAsync(_flights);
            return true;
        }

        return false;
    }

    public bool ShowMyFlights(int userId)
    {
        var myFlights = _flights.Where(flight => flight.PassengerId == userId).ToList();
        if (!myFlights.Any())
        {
            return false;
        }
        _bookingManager.DisplayFlights(myFlights);
        return true;
    }

    public async Task<List<string>?> ImportFlightsFromCsvAsync(string filePath)
    {
        var imports = await _repository.ImportFlightsAsync(filePath);  
        _flights = _flights.Concat((imports["Flights"] as List<Flight>)!).ToList();
        await _repository.UpdateAsync(_flights);  
        return (List<string>?)imports["Errors"];
    }
}