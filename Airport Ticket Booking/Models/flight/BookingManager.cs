namespace Airport_Ticket_Booking.Models.flight;

public class BookingManager : IBookingManager
{
    public bool Book(List<Flight> flights, int flightId,int userId)
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
            result.PassengerId = userId;
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
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
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public bool ModifyClass(List<Flight> flights, int flightId, int classNumber, int userId)
    {
        try
        {
            var flight=flights.SingleOrDefault(flight => flight.FlightId == flightId && flight.IsBook && flight.PassengerId == userId);
            if (flight == null)
            {
                return false;
            }
            if ((int)flight.Class == classNumber)
            {
                return true;
            }
            flight.Price= flight.Class.CalculateFlightPrice(flight.Price, (FlightClass)classNumber);
            flight.Class = (FlightClass)classNumber;
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
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