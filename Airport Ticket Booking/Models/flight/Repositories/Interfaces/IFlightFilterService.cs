namespace Airport_Ticket_Booking.Models.flight.Repositories.Interfaces;

public interface IFlightFilterService
{
    public List<Flight> FilterFlights(List<Flight> flights, int? flightId = null, decimal? price = null,
        string? departureCountry = null,
        string? destinationCountry = null, DateTime? departureDate = null, string? departureAirport = null,
        string? arrivalAirport = null, int? passenger = null, int flightClass = 0);
}