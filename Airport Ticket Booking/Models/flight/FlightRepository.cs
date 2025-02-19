namespace Airport_Ticket_Booking.Models.flight;

public class FlightRepository(string filePath) : IFlightRepository
{
    private readonly List<Flight> _flights = [];


    public List<Flight> GetAllData()
    {
        try
        {
            if (_flights.Count > 0) return _flights;
            _flights.AddRange(File.ReadAllLines(filePath)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(line => line.Split(','))
                .Where(flight => flight.Length >= 7)
                .Select(flight =>
                {
                    var departureDate = DataSplitting(flight, out var price, out var departureCountry,
                        out var destinationCountry, out var departureAirport, out var arrivalAirport,
                        out var passengerId, out var @class, out var flightId);
                    return new Flight(departureDate, price, departureCountry, destinationCountry, arrivalAirport,
                        departureAirport,
                        @class, passengerId, flightId)
                    {
                        DepartureAirport = departureAirport,
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

    private static DateTime DataSplitting(string[] flight, out decimal price, out string departureCountry,
        out string destinationCountry, out string departureAirport, out string arrivalAirport, out string? passengerId,
        out FlightClass @class, out int flightId)
    {
        var departureDate = DateTime.Parse(flight[0]);
        price = decimal.Parse(flight[1]);
        departureCountry = flight[2];
        destinationCountry = flight[3];
        departureAirport = flight[4];
        arrivalAirport = flight[5];
        @class = Enum.Parse<FlightClass>(flight[6]);
        passengerId =flight[7];
        flightId = int.Parse(flight[8]);
        return departureDate;
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