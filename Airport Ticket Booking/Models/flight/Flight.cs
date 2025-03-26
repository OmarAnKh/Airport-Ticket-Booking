using System.ComponentModel.DataAnnotations;
using Airport_Ticket_Booking.Models.flight.Attribute;

namespace Airport_Ticket_Booking.Models.flight;

public class Flight
{
    public Flight(DateTime departureDate, decimal price, string departureCountry, string destinationCountry,
        string departureAirport, string arrivalAirport, FlightClass flightClass, bool isBook, int? passengerId, int flightId = 0)
    {
        DepartureDate = departureDate;
        Price = price;
        DepartureCountry = departureCountry;
        DestinationCountry = destinationCountry;
        DepartureAirport = departureAirport;
        ArrivalAirport = arrivalAirport;
        PassengerId = passengerId;
        Class = flightClass;
        FlightId = flightId;
        IsBook = isBook;
    }

    public int FlightId { get; init; }
    [Range(0, (double)decimal.MaxValue, ErrorMessage = "Price cannot be negative")]
    public decimal Price{get;set;}
    [FutureDate(ErrorMessage = "Departure date must be today or in the future.")]
    public DateTime DepartureDate { get; init; }
    public bool IsBook { get; set; }

    public string DepartureCountry { get; init; }
    public string DepartureAirport { get; init; }
    public string DestinationCountry { get; init; }
    public string ArrivalAirport { get; init; }
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
    
    public override string ToString()
    {
        return $"Flight ID: {FlightId}, Departure Date: {DepartureDate:yyyy-MM-dd}, " +
               $"Departure Country: {DepartureCountry}, Destination Country: {DestinationCountry}, " +
               $"Departure Airport: {DepartureAirport}, Arrival Airport: {ArrivalAirport}, " +
               $"Class: {Class}, Price: {Price:C}, Booked: {IsBook}";
    }
}