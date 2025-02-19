namespace Airport_Ticket_Booking.Models.flight;

public class BookingManager : IBookingManager
{
    public bool Book(List<Flight> flights, int flightId)
    {
        try
        {
            var result = flights
                .SingleOrDefault(flight => flight.FlightId == flightId && flight.IsBook == false);
            if (result == null)
            {
                return false;
            }

            result.IsBook = true;
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public bool Cancel(List<Flight> flights, int flightId)
    {
        try
        {
            var result = flights
                .SingleOrDefault(flight => flight.FlightId == flightId && flight.IsBook);
            if (result == null)
            {
                return false;
            }

            result.IsBook = false;
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public bool Modify()
    {
        throw new NotImplementedException();
    }

    public void DisplayFlights(List<Flight> flights)
    {
        foreach (var flight in flights)
        {
            Console.WriteLine(
                $"Flight: {flight.FlightId}, Departure Date: {flight.DepartureDate}, Departure Country:{flight.DepartureCountry}, " +
                $"Destination Country:{flight.DestinationCountry}, Departure Airport: {flight.DepartureAirport}, Arrival Airport: {flight.ArrivalAirport}" +
                $", Flight Class: {flight.Class}\n");
        }
    }
}