namespace Airport_Ticket_Booking.Models.flight.Services.interfaces;

public interface IFlightFilterService
{
    List<Flight> FilterFlights(List<Flight> flights, FlightFilterCriteria filterCriteria);
}