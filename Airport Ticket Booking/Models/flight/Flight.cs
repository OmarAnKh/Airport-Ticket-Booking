namespace Airport_Ticket_Booking.Models.flight;

public class Flight
{
    public Flight(DateTime departureDate, int price, string? departureCountry, string? destinationCountry,
        string? arrivalAirport, FlightClass @class, string? passengerId = null, int flightId = 0)
    {
        DepartureDate = departureDate;
        Price = price;
        DepartureCountry = departureCountry;
        DestinationCountry = destinationCountry;
        ArrivalAirport = arrivalAirport;
        PassengerId = passengerId;
        Class = @class;
        FlightId = flightId;
    }

    public int FlightId { get; init; }
    private int _price;
    private DateTime _departureDate;


    public required string? DepartureCountry { get; init; }
    public required string? DestinationCountry { get; init; }
    public required string? ArrivalAirport { get; init; }
    public string? PassengerId { get; set; }
    public FlightClass Class { get; set; }

    public required int Price
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
}