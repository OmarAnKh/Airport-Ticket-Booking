using Airport_Ticket_Booking.Models.flight.Services.interfaces;

namespace Airport_Ticket_Booking.Models.flight.Services;

public class FlightFilterService : IFlightFilterService
{
    public List<Flight> FilterFlights(List<Flight> flights, FlightFilterCriteria filterCriteria)
    {
        return flights.Where(flight =>
            (!filterCriteria.FlightId.HasValue || flight.FlightId == filterCriteria.FlightId) &&
            (!filterCriteria.Price.HasValue || flight.Price == filterCriteria.Price) &&
            (string.IsNullOrEmpty(filterCriteria.DepartureCountry) || flight.DepartureCountry == filterCriteria.DepartureCountry) &&
            (string.IsNullOrEmpty(filterCriteria.DestinationCountry) || flight.DestinationCountry == filterCriteria.DestinationCountry) &&
            (!filterCriteria.DepartureDate.HasValue || flight.DepartureDate.Date == filterCriteria.DepartureDate.Value.Date) &&
            (string.IsNullOrEmpty(filterCriteria.DepartureAirport) || flight.DepartureAirport == filterCriteria.DepartureAirport) &&
            (string.IsNullOrEmpty(filterCriteria.ArrivalAirport) || flight.ArrivalAirport == filterCriteria.ArrivalAirport) &&
            (!filterCriteria.Passenger.HasValue || flight.PassengerId == filterCriteria.Passenger) &&
            (!filterCriteria.FlightClass.HasValue || flight.Class == (FlightClass)filterCriteria.FlightClass)
        ).ToList();
    }

}