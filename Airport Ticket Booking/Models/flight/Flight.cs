namespace Airport_Ticket_Booking.Models.flight;

public class Flight
{
    public Flight(DateTime departureDate, decimal price, string? departureCountry, string? destinationCountry,
        string departureAirport,
        string? arrivalAirport, FlightClass @class, bool isBook, int passengerId = 0, int flightId = 0)
    {
        DepartureDate = departureDate;
        Price = price;
        DepartureCountry = departureCountry;
        DestinationCountry = destinationCountry;
        DepartureAirport = departureAirport;
        ArrivalAirport = arrivalAirport;
        PassengerId = passengerId;
        Class = @class;
        FlightId = flightId;
        IsBook = isBook;
    }

    public int FlightId { get; init; }
    private decimal _price;
    private DateTime _departureDate;
    public bool IsBook { get; set; }


    public required string? DepartureCountry { get; init; }
    public required string? DepartureAirport { get; init; }
    public required string? DestinationCountry { get; init; }
    public required string? ArrivalAirport { get; init; }
    public int PassengerId { get; set; }
    public FlightClass Class { get; set; }

    public required decimal Price
    {
        get => _price;
        set
        {
            if (value < 0)
            {
                throw new ArgumentException("Price cannot be negative");
            }

            _price = value;
        }
    }

    public required DateTime DepartureDate
    {
        get => _departureDate;
        set
        {
            if (value >= DateTime.Now.Date)
            {
                _departureDate = value;
            }
            else
            {
                throw new ArgumentException("Departure date must be today or in the future.");
            }
        }
    }

    public bool Equals(Flight? obj)
    {
        if (obj is null)
            return false;

        return DepartureDate == obj.DepartureDate &&
               Price == obj.Price &&
               DepartureCountry == obj.DepartureCountry &&
               DestinationCountry == obj.DestinationCountry &&
               DepartureAirport == obj.DepartureAirport &&
               ArrivalAirport == obj.ArrivalAirport &&
               Class == obj.Class &&
               FlightId == obj.FlightId;
    }
}