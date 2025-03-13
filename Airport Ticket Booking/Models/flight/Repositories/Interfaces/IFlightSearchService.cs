namespace Airport_Ticket_Booking.Models.flight.Repositories.Interfaces;

public interface IFlightSearchService
{
    public List<Flight> SearchFlights(List<Flight> flights, string? departureCountry = null,
        string? destinationCountry = null,
        DateTime? departureDate = null, string? departureAirport = null,
        string? arrivalAirport = null, string? flightClass = null,
        decimal? maxPrice = null);

  
}