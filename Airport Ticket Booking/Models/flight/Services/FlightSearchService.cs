using Airport_Ticket_Booking.Models.flight.Repositories.Interfaces;

namespace Airport_Ticket_Booking.Models.flight;

public class FlightSearchService : IFlightSearchService
{
    public List<Flight> SearchFlights(List<Flight> flights, string? departureCountry = null,
        string? destinationCountry = null, DateTime? departureDate = null,
        string? departureAirport = null, string? arrivalAirport = null, string? flightClass = null,
        decimal? maxPrice = null)
    {
        var searchResults = flights.Where(flight =>
            CheckSearchCriteria(departureCountry, destinationCountry, departureDate, departureAirport,
                arrivalAirport, flightClass, maxPrice, flight)).ToList();
        return searchResults;
    }

    private static bool CheckSearchCriteria(string? departureCountry, string? destinationCountry,
        DateTime? departureDate, string? departureAirport, string? arrivalAirport, string? flightClass,
        decimal? maxPrice, Flight flight)
    {
        return ((string.IsNullOrEmpty(departureCountry) || flight.DepartureCountry == departureCountry) &&
                (string.IsNullOrEmpty(destinationCountry) || flight.DestinationCountry == destinationCountry) &&
                (!departureDate.HasValue || flight.DepartureDate.Date == departureDate.Value.Date) &&
                (string.IsNullOrEmpty(departureAirport) || flight.DepartureAirport == departureAirport) &&
                (string.IsNullOrEmpty(arrivalAirport) || flight.ArrivalAirport == arrivalAirport) &&
                (string.IsNullOrEmpty(flightClass) || 
                 (Enum.TryParse<FlightClass>(flightClass, out var parsedClass) && flight.Class == parsedClass)) &&
                (!maxPrice.HasValue || flight.Price <= maxPrice) &&
                !flight.IsBook);
    }
}