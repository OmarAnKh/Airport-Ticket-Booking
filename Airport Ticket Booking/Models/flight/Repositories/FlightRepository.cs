using Airport_Ticket_Booking.Models.flight.Repositories.Interfaces;

namespace Airport_Ticket_Booking.Models.flight.Repositories;

public class FlightRepository : IFlightRepository
{
    private readonly string _filePath;
    private static readonly Lock Lock = new Lock();
    private static FlightRepository? _instance;

    private FlightRepository(string filePath)
    {
        _filePath = filePath;
    }

    public static FlightRepository GetInstance(string filePath)
    {
        if (_instance == null)
        {
            lock (Lock)
            {
                _instance ??= new FlightRepository(filePath);
            }
        }

        return _instance;
    }

    public List<Flight> GetAllData()
    {
        var flights = new List<Flight>();
        try
        {
            flights.AddRange(File.ReadAllLines(_filePath)
                    .Where(line => !string.IsNullOrWhiteSpace(line))
                    .Select(line => line.Split(','))
                    .Where(flight => flight.Length >= 10)
                    .Select(DataSplitting)
            );
        }
        catch (Exception ex)
        {
            throw new Exception($"could not load flights {ex.Message}");
        }

        return flights;
    }


    private static Flight DataSplitting(string[] flight)
    {
        var departureDate = DateTime.Parse(flight[0]);
        var price = decimal.Parse(flight[1]);
        var departureCountry = flight[2];
        var destinationCountry = flight[3];
        var departureAirport = flight[4];
        var arrivalAirport = flight[5];
        var flightClass = Enum.Parse<FlightClass>(flight[6]);
        var isBook = bool.Parse(flight[7]);
        var passengerId = string.IsNullOrWhiteSpace(flight[8]) ? (int?)null : int.Parse(flight[8]);
        var flightId = int.Parse(flight[9]);

        return new Flight(departureDate, price, departureCountry, destinationCountry, departureAirport,
            arrivalAirport,flightClass, isBook, passengerId, flightId);

    }

    public async Task UpdateAsync(List<Flight> flights)
    {
        using StreamWriter sw = new StreamWriter(_filePath);
        foreach (var flight in flights)
        {
            await sw.WriteLineAsync(
                $"{flight.DepartureDate},{flight.Price},{flight.DepartureCountry},{flight.DestinationCountry}," +
                $"{flight.DepartureAirport},{flight.ArrivalAirport},{flight.Class},{flight.IsBook}," +
                $"{flight.PassengerId},{flight.FlightId}");
        }

        await sw.FlushAsync();
    }

public async Task<Dictionary<string, object?>> ImportFlightsAsync(string importFilePath)
{
    const int ExpectedFieldCount = 10;  // Declare as constant for better readability
    List<string> errors = new List<string>();
    List<Flight>? importedFlights = new List<Flight>();

    if (!File.Exists(importFilePath))
    {
        Console.WriteLine($"Error: File '{importFilePath}' not found.");
        errors.Add($"Error: File '{importFilePath}' not found.");
        return new Dictionary<string, object?>
        {
            { "Errors", errors },
            { "Flights", importedFlights }
        };
    }

    using StreamReader sr = new StreamReader(importFilePath);
    int lineNumber = 1;

    while (await sr.ReadLineAsync() is { } line)
    {
        var flightData = line.Split(',');
        List<string> lineErrors = new List<string>();

        if (flightData.Length < ExpectedFieldCount)
        {
            lineErrors.Add(
                $"Line {lineNumber}: Incomplete flight data. Expected {ExpectedFieldCount} fields, found {flightData.Length}.");
        }

        DateTime departureDate = ValidationHelper.ValidateDate(flightData[0], lineNumber, errors);
        decimal price = ValidationHelper.ValidateDecimal(flightData[1], "Price", lineNumber, errors);
        string departureCountry = ValidationHelper.ValidateString(flightData[2], "Departure Country", lineNumber, errors);
        string destinationCountry = ValidationHelper.ValidateString(flightData[3], "Destination Country", lineNumber, errors);
        string departureAirport = ValidationHelper.ValidateString(flightData[4], "Departure Airport", lineNumber, errors);
        string arrivalAirport = ValidationHelper.ValidateString(flightData[5], "Arrival Airport", lineNumber, errors);
        FlightClass flightClass = ValidationHelper.ValidateEnum<FlightClass>(flightData[6], "Flight Class", lineNumber, errors);
        bool isBook = ValidationHelper.ValidateBool(flightData[7], "IsBook", lineNumber, errors);
        int? passengerId = ValidationHelper.ValidateNullableInt(flightData[8], "Passenger ID", lineNumber, errors);
        int flightId = ValidationHelper.ValidateInt(flightData[9], "Flight ID", lineNumber, errors);

        if (lineErrors.Count > 0)
        {
            errors.AddRange(lineErrors);
        }
        else
        {
            importedFlights?.Add(new Flight(departureDate, price, departureCountry, destinationCountry,
                departureAirport, arrivalAirport, flightClass, isBook, passengerId, flightId));
        }

        lineNumber++;
    }

    Console.WriteLine("Data imported successfully.");
    return new Dictionary<string, object?>
    {
        { "Errors", errors },
        { "Flights", importedFlights }
    };
}

}