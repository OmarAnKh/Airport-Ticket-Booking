namespace Airport_Ticket_Booking.Models.flight;

public class FlightSearchServices : IFlightSearchServices
{
    public List<Flight> SearchFlights(List<Flight> flights, string? departureCountry = null,
        string? destinationCountry = null, DateTime? departureDate = null,
        string? departureAirport = null, string? arrivalAirport = null, string? flightClass = null,
        decimal? maxPrice = null)
    {
        var searchResults = flights.Where(
            flight => ( flight.DepartureCountry == departureCountry) &&
                      (string.IsNullOrEmpty(destinationCountry) || flight.DestinationCountry == destinationCountry) &&
                      (flight.DepartureDate == departureDate) &&
                      (flight.DepartureAirport == departureAirport) &&
                      (flight.ArrivalAirport == arrivalAirport) &&
                      ((Enum.TryParse<FlightClass>(flightClass,
                              out var parsedClass) && flight.Class == parsedClass) &&
                          (!maxPrice.HasValue || flight.Price <= maxPrice) &&
                          flight.IsBook == false)).ToList();
        return searchResults;
    }
}