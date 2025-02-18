namespace Airport_Ticket_Booking.Models.flight;

public class FlightRepository : IFlightRepository
{
    private readonly string _filePath;
    private readonly List<Flight> _flights;

    public FlightRepository(string filePath)
    {
        _filePath = filePath;
        _flights = [];
        _flights = GetAllData();
    }

    public List<Flight> GetAllData()
    {
        try
        {
            if (_flights.Count > 0) return _flights;
            _flights.AddRange(File.ReadAllLines(_filePath)
                    .Where(line => !string.IsNullOrWhiteSpace(line))
                    .Select(line => line.Split(','))
                    .Where(flight => flight.Length >= 7)
                    .Select(flight =>
                    {
                        var departureDate = DateTime.Parse(flight[0]);
                        var price = int.Parse(flight[1]);
                        var departureCountry = flight[2];
                        var destinationCountry = flight[3];
                        var arrivalAirport = flight[4];
                        var passengerId = flight[5].Length >= 1 ? flight[5] : null;
                        var @class = Enum.Parse<FlightClass>(flight[6]);
                        var flightId = flight.Length > 7 ? int.Parse(flight[7]) : 0;
                        return new Flight(departureDate, price, departureCountry, destinationCountry, arrivalAirport,
                            @class, passengerId, flightId)
                        {
                            DepartureCountry = departureCountry,
                            DestinationCountry = destinationCountry,
                            ArrivalAirport = arrivalAirport,
                            Price = price,
                            DepartureDate = departureDate
                        };
                    })
            );
        }
        catch (Exception ex)
        {
            throw new Exception($"could not load flights {ex.Message}");
        }

        return _flights;
    }

    public Flight? GetById(int flightId)
    {
        throw new NotImplementedException();
    }

    public void Create(Flight flight)
    {
        throw new NotImplementedException();
    }

    public void Update(Flight flight)
    {
        throw new NotImplementedException();
    }

    public void Delete(Flight flight)
    {
        throw new NotImplementedException();
    }

    public void ImportFlights(string filePath)
    {
        throw new NotImplementedException();
    }
}