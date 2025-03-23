namespace Airport_Ticket_Booking.Models.flight.Services.interfaces;

public interface IFlightSearchService
{
    List<Flight> SearchFlights(List<Flight> flights, FlightSearchCriteria flightSearchCriteria);

  
}