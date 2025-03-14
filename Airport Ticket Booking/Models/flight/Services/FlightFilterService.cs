using Airport_Ticket_Booking.Models.flight.Repositories.Interfaces;

namespace Airport_Ticket_Booking.Models.flight.Services;

public class FlightFilterService : IFlightFilterService
{
    public List<Flight> FilterFlights(List<Flight> flights, int? flightId = null, decimal? price = null,
        string? departureCountry = null,
        string? destinationCountry = null, DateTime? departureDate = null, string? departureAirport = null,
        string? arrivalAirport = null, int? passenger = null, int flightClass = -1)
    {
        return flights.Where(flight =>
            (!flightId.HasValue || flight.FlightId == flightId) &&
            (!price.HasValue || flight.Price == price) &&
            (string.IsNullOrEmpty(departureCountry) || flight.DepartureCountry == departureCountry) &&
            (string.IsNullOrEmpty(destinationCountry) || flight.DestinationCountry == destinationCountry) &&
            (!departureDate.HasValue || flight.DepartureDate.Date == departureDate.Value.Date) &&
            (string.IsNullOrEmpty(departureAirport) || flight.DepartureAirport == departureAirport) &&
            (string.IsNullOrEmpty(arrivalAirport) || flight.ArrivalAirport == arrivalAirport) &&
            (!passenger.HasValue || flight.PassengerId == passenger) &&
            (flightClass == -1 || flight.Class == (FlightClass)flightClass)
        ).ToList();
    }
}