namespace Airport_Ticket_Booking.Models.flight;

public interface IFlightSearchServices
{
    public List<Flight> SearchFlights(List<Flight> flights, string? departureCountry = null,
        string? destinationCountry = null,
        DateTime? departureDate = null, string? departureAirport = null,
        string? arrivalAirport = null, string? flightClass = null,
        decimal? maxPrice = null);
}