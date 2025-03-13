using Airport_Ticket_Booking.Models.flight.Repositories.Interfaces;

namespace Airport_Ticket_Booking.Models.flight;

public class FlightFilterService : IFlightFilterService
{
    public List<Flight> FilterFlights(List<Flight> flights, int? flightId = null, decimal? price = null,
        string? departureCountry = null,
        string? destinationCountry = null, DateTime? departureDate = null, string? departureAirport = null,
        string? arrivalAirport = null, int? passenger = null, int flightClass = 0)
    {
        var filterFlights = flights.Where(b =>
            (b.FlightId == flightId) ||
            (b.Price == price) ||
            (b.DepartureCountry == departureCountry) ||
            (b.DestinationCountry == destinationCountry) ||
            (b.DepartureDate == departureDate) ||
            (b.DepartureAirport == departureAirport) ||
            (b.ArrivalAirport == arrivalAirport) ||
            (b.PassengerId == passenger) ||
            (b.Class == (FlightClass)flightClass)
        ).ToList();

        return filterFlights;
    }
}