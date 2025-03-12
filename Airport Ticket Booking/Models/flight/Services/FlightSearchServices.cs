using Airport_Ticket_Booking.Models.flight.Repositories.Interfaces;

namespace Airport_Ticket_Booking.Models.flight;

public class FlightSearchServices : IFlightSearchServices
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

    private static bool CheckSearchCriteria(string? departureCountry, string? destinationCountry,
        DateTime? departureDate, string? departureAirport, string? arrivalAirport, string? flightClass,
        decimal? maxPrice, Flight flight)
    {
        return (flight.DepartureCountry == departureCountry) &&
               (string.IsNullOrEmpty(destinationCountry) || flight.DestinationCountry == destinationCountry) &&
               (flight.DepartureDate == departureDate) &&
               (flight.DepartureAirport == departureAirport) &&
               (flight.ArrivalAirport == arrivalAirport) &&
               ((Enum.TryParse<FlightClass>(flightClass,
                    out var parsedClass) && flight.Class == parsedClass) &&
                (!maxPrice.HasValue || flight.Price <= maxPrice) &&
                flight.IsBook == false);
    }
}