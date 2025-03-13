using Airport_Ticket_Booking.Models.flight.Repositories.Interfaces;

namespace Airport_Ticket_Booking.Models.flight;

public class FlightSearchService : IFlightSearchService
{
    public List<Flight> SearchFlights(List<Flight> flights, string? departureCountry = null,
        string? destinationCountry = null, DateTime? departureDate = null,
        string? departureAirport = null, string? arrivalAirport = null, string? flightClass = null,
        decimal? maxPrice = null)
    {
        var searchResults = flights.Where(
            flight => CheckSearchCriteria(departureCountry, destinationCountry, departureDate, departureAirport,
                arrivalAirport, flightClass, maxPrice, flight)).ToList();
        return searchResults;
    }

    private static bool CheckSearchCriteria(string? departureCountry, string? destinationCountry,
        DateTime? departureDate, string? departureAirport, string? arrivalAirport, string? flightClass,
        decimal? maxPrice, Flight flight)
    {
        return (flight.DepartureCountry == departureCountry) ||
               (string.IsNullOrEmpty(destinationCountry) || flight.DestinationCountry == destinationCountry) ||
               (flight.DepartureDate == departureDate) ||
               (flight.DepartureAirport == departureAirport) ||
               (flight.ArrivalAirport == arrivalAirport) ||
               ((Enum.TryParse<FlightClass>(flightClass,
                    out var parsedClass) && flight.Class == parsedClass) ||
                (!maxPrice.HasValue || flight.Price <= maxPrice) ||
                flight.IsBook == false);
    }
}