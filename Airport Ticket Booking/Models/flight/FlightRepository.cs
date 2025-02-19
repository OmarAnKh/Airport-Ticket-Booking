namespace Airport_Ticket_Booking.Models.flight;

public class FlightRepository(string filePath) : IFlightRepository
{
    public List<Flight> GetAllData(List<Flight> flights)
    {
        try
        {
            if (flights.Count > 0) return flights;
            flights.AddRange(File.ReadAllLines(filePath)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(line => line.Split(','))
                .Where(flight => flight.Length >= 7)
                .Select(flight =>
                {
                    var departureDate = DataSplitting(flight, out var price, out var departureCountry,
                        out var destinationCountry, out var departureAirport, out var arrivalAirport,
                        out var passengerId, out var @class, out var isBook, out var flightId);
                    return new Flight(departureDate, price, departureCountry, destinationCountry, departureAirport,
                        arrivalAirport,
                        @class, isBook, passengerId, flightId)
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

        return flights;
    }

    private static DateTime DataSplitting(string[] flight, out decimal price, out string departureCountry,
        out string destinationCountry, out string departureAirport, out string arrivalAirport, out string? passengerId,
        out FlightClass @class, out bool isBook, out int flightId)
    {
        var departureDate = DateTime.Parse(flight[0]);
        price = decimal.Parse(flight[1]);
        departureCountry = flight[2];
        destinationCountry = flight[3];
        departureAirport = flight[4];
        arrivalAirport = flight[5];
        @class = Enum.Parse<FlightClass>(flight[6]);
        isBook = bool.Parse(flight[7]);
        passengerId = flight[8];
        flightId = int.Parse(flight[9]);
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

    public void Update(List<Flight> flights, string filePath)
    {
        using StreamWriter sw = new StreamWriter(filePath);
        foreach (var flight in flights)
        {
            sw.WriteLine(
                $"{flight?.DepartureDate},{flight?.Price},{flight?.DepartureCountry},{flight?.DestinationCountry}," +
                $"{flight?.DepartureAirport},{flight?.ArrivalAirport},{flight?.Class},{flight?.IsBook}," +
                $"{flight?.PassengerId},{flight!.FlightId}");
        }

        sw.Close();
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