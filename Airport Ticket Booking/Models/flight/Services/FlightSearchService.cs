using Airport_Ticket_Booking.Models.flight.Repositories.Interfaces;
using Airport_Ticket_Booking.Models.flight.Services.interfaces;

namespace Airport_Ticket_Booking.Models.flight.Services;

public class FlightSearchService : IFlightSearchService
{
    public List<Flight> SearchFlights(List<Flight> flights, FlightSearchCriteria searchCriteria)
    {
        var searchResults = flights.Where(flight =>
            IsMatchingSearchCriteria(searchCriteria, flight)).ToList();
        return searchResults;
    }
    private static bool IsMatchingSearchCriteria (FlightSearchCriteria searchCriteria, Flight flight)
    {
        return ((string.IsNullOrEmpty(searchCriteria.DepartureCountry) || flight.DepartureCountry == searchCriteria.DepartureCountry) &&
                (string.IsNullOrEmpty(searchCriteria.DestinationCountry) || flight.DestinationCountry == searchCriteria.DestinationCountry) &&
                (!searchCriteria.DepartureDate.HasValue || flight.DepartureDate.Date == searchCriteria.DepartureDate.Value.Date) &&
                (string.IsNullOrEmpty(searchCriteria.DepartureAirport) || flight.DepartureAirport == searchCriteria.DepartureAirport) &&
                (string.IsNullOrEmpty(searchCriteria.ArrivalAirport) || flight.ArrivalAirport == searchCriteria.ArrivalAirport) &&
                (!searchCriteria.FlightClass.HasValue || flight.Class == searchCriteria.FlightClass) && 
                (!searchCriteria.MaxPrice.HasValue || flight.Price <= searchCriteria.MaxPrice) &&
                !flight.IsBook);
    }



}