namespace Airport_Ticket_Booking.Models.flight;

public class FlightSearchServices : IFlightSearchServices
{
    public List<Flight> SearchFlights(List<Flight> flights, string? departureCountry = null,
        string? destinationCountry = null, DateTime? departureDate = null,
        string? departureAirport = null, string? arrivalAirport = null, string? flightClass = null,
        decimal? maxPrice = null)
    {
        var searchResults = flights.Where(
            flight => (string.IsNullOrEmpty(departureCountry) || flight.DepartureCountry == departureCountry) &&
                      (string.IsNullOrEmpty(destinationCountry) || flight.DestinationCountry == destinationCountry) &&
                      (string.IsNullOrEmpty(departureDate.ToString()) || flight.DepartureDate == departureDate) &&
                      (string.IsNullOrEmpty(departureAirport) || flight.DepartureAirport == departureAirport) &&
                      (string.IsNullOrEmpty(arrivalAirport) || flight.ArrivalAirport == arrivalAirport) &&
                      (string.IsNullOrEmpty(flightClass) || (Enum.TryParse<FlightClass>(flightClass,
                              out var parsedClass) && flight.Class == parsedClass) &&
                          (!maxPrice.HasValue || flight.Price <= maxPrice))).ToList();
        return searchResults;
    }
}