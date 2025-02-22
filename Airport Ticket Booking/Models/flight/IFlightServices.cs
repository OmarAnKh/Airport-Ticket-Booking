namespace Airport_Ticket_Booking.Models.flight;

    public interface IFlightServices
    {
        void SearchFlights(string? departureCountry = null,
            string? destinationCountry = null, DateTime? departureDate = null,
            string? departureAirport = null, string? arrivalAirport = null, 
            string? flightClass = null, decimal? maxPrice = null);

        bool BookFlight(int flightId, int userId);

        void CancelFlight(int flightId);

        void ShowMyFlights();
    }

