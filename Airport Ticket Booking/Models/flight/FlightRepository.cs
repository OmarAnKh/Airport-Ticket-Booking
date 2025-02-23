namespace Airport_Ticket_Booking.Models.flight;

public class FlightRepository : IFlightRepository
{
    private readonly string _filePath;
    private readonly static Lock Lock = new Lock();
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

    public List<Flight> GetAllData(List<Flight> flights)
    {
        try
        {
            if (flights.Count > 0) return flights;
            flights.AddRange(File.ReadAllLines(_filePath)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(line => line.Split(','))
                .Where(flight => flight.Length >= 7)
                .Select(flight =>
                {
                    var departureDate = DataSplitting(flight, out var price, out var departureCountry,
                        out var destinationCountry, out var departureAirport, out var arrivalAirport,
                        out int? passengerId, out var @class, out var isBook, out var flightId);
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
        out string destinationCountry, out string departureAirport, out string arrivalAirport, out int? passengerId,
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
        passengerId = string.IsNullOrWhiteSpace(flight[8]) ? null : int.Parse(flight[8]);
        flightId = int.Parse(flight[9]);
        return departureDate;
    }

    public void Update(List<Flight> flights)
    {
        using StreamWriter sw = new StreamWriter(_filePath);
        foreach (var flight in flights)
        {
            sw.WriteLine(
                $"{flight.DepartureDate},{flight.Price},{flight.DepartureCountry},{flight.DestinationCountry}," +
                $"{flight.DepartureAirport},{flight.ArrivalAirport},{flight.Class},{flight.IsBook}," +
                $"{flight.PassengerId},{flight.FlightId}");
        }

        sw.Close();
    }

    public Dictionary<string, object?> ImportFlights(string importFilePath)
    {
        List<string> errors = [];
        List<Flight>? importedFlights = [];

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
        importedFlights = [];
        using StreamReader sr = new StreamReader(importFilePath);
        int lineNumber = 1;

        while (sr.ReadLine() is { } line)
        {
            var flightData = line.Split(',');
            int errorCounter = 0;

            if (flightData.Length < 10)
            {
                errors.Add(
                    $"Line {lineNumber}: Incomplete flight data. Expected 10 fields, found {flightData.Length}.");
                lineNumber++;
                continue;
            }

            DateTime departureDate = ValidateDate(flightData[0], lineNumber, errors, ref errorCounter);
            decimal price = ValidateDecimal(flightData[1], "Price", lineNumber, errors, ref errorCounter);
            string departureCountry =
                ValidateString(flightData[2], "Departure Country", lineNumber, errors, ref errorCounter);
            string destinationCountry =
                ValidateString(flightData[3], "Destination Country", lineNumber, errors, ref errorCounter);
            string departureAirport =
                ValidateString(flightData[4], "Departure Airport", lineNumber, errors, ref errorCounter);
            string arrivalAirport =
                ValidateString(flightData[5], "Arrival Airport", lineNumber, errors, ref errorCounter);
            FlightClass @class =
                ValidateEnum<FlightClass>(flightData[6], "Flight Class", lineNumber, errors, ref errorCounter);
            bool isBook = ValidateBool(flightData[7], "IsBook", lineNumber, errors, ref errorCounter);
            int? passengerId = ValidateNullableInt(flightData[8], "Passenger ID", lineNumber, errors, ref errorCounter);
            int flightId = ValidateInt(flightData[9], "Flight ID", lineNumber, errors, ref errorCounter);

            if (errorCounter == 0)
            {
                importedFlights?.Add(new Flight(departureDate, price, departureCountry, destinationCountry,
                    departureAirport, arrivalAirport, @class, isBook, passengerId, flightId)
                {
                    DepartureCountry = departureCountry,
                    DepartureAirport = departureAirport,
                    DestinationCountry = destinationCountry,
                    ArrivalAirport = arrivalAirport,
                    Price = price,
                    DepartureDate = departureDate
                });
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

    private DateTime ValidateDate(string value, int line, List<string> errors, ref int errorCounter)
    {
        if (DateTime.TryParse(value, out var date))
        {
            if (date < DateTime.Now)
            {
                errors.Add($"Line {line}: Departure date '{value}' cannot be in the past.");
                errorCounter++;
            }

            return date;
        }

        errors.Add($"Line {line}: Invalid date format '{value}'.");
        errorCounter++;
        return DateTime.MinValue;
    }

    private decimal ValidateDecimal(string value, string fieldName, int line, List<string> errors, ref int errorCounter)
    {
        if (decimal.TryParse(value, out var result) && result >= 0)
        {
            return result;
        }

        errors.Add($"Line {line}: Invalid {fieldName} '{value}', must be a positive number.");
        errorCounter++;
        return 0;
    }

    private string ValidateString(string value, string fieldName, int line, List<string> errors, ref int errorCounter)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            errors.Add($"Line {line}: {fieldName} cannot be empty.");
            errorCounter++;
        }

        return value.Trim();
    }

    private int ValidateInt(string value, string fieldName, int line, List<string> errors, ref int errorCounter)
    {
        if (int.TryParse(value, out var result))
        {
            return result;
        }

        errors.Add($"Line {line}: Invalid {fieldName} '{value}', must be an integer.");
        errorCounter++;
        return 0;
    }

    private int? ValidateNullableInt(string value, string fieldName, int line, List<string> errors,
        ref int errorCounter)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return ValidateInt(value, fieldName, line, errors, ref errorCounter);
    }

    private T ValidateEnum<T>(string value, string fieldName, int line, List<string> errors, ref int errorCounter)
        where T : struct
    {
        if (Enum.TryParse(value, true, out T result))
        {
            return result;
        }

        errors.Add($"Line {line}: Invalid {fieldName} '{value}', must be a valid {typeof(T).Name}.");
        errorCounter++;
        return default!;
    }

    private bool ValidateBool(string value, string fieldName, int line, List<string> errors, ref int errorCounter)
    {
        if (bool.TryParse(value, out var result))
        {
            return result;
        }

        errors.Add($"Line {line}: Invalid {fieldName} '{value}', must be 'true' or 'false'.");
        errorCounter++;
        return false;
    }
}