namespace Airport_Ticket_Booking.Models.flight;

public class BookingManager : IBookingManager
{
    public bool Book(List<Flight> flights, int flightId)
    {
        try
        {
            var result = flights.SingleOrDefault(flight => flight.FlightId == flightId);
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

    public bool Cancel()
    {
        throw new NotImplementedException();
    }

    public bool Modify()
    {
        throw new NotImplementedException();
    }
}