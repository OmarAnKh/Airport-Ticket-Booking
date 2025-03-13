using System.ComponentModel.DataAnnotations;
using Airport_Ticket_Booking.Models.flight.Attribute;

namespace Airport_Ticket_Booking.Models.flight;

public class Flight
{
    public Flight(DateTime departureDate, decimal price, string? departureCountry, string? destinationCountry,
        string departureAirport,
        string? arrivalAirport, FlightClass @class, bool isBook, int? passengerId, int flightId = 0)
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
    [Range(0, (double)decimal.MaxValue, ErrorMessage = "Price cannot be negative")]
    public decimal Price{get;set;}
    [FutureDate(ErrorMessage = "Departure date must be today or in the future.")]
    public DateTime DepartureDate { get; init; }
    public bool IsBook { get; set; }


    public required string? DepartureCountry { get; init; }
    public required string? DepartureAirport { get; init; }
    public required string? DestinationCountry { get; init; }
    public required string? ArrivalAirport { get; init; }
    public int? PassengerId { get; set; }
    public FlightClass Class { get; set; }

    
    

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