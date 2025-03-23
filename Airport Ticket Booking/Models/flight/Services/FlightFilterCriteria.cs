namespace Airport_Ticket_Booking.Models.flight.Services
{
    public class FlightFilterCriteria
    {
        public int? FlightId { get; set; }
        public decimal? Price { get; set; }
        public string? DepartureCountry { get; set; }
        public string? DestinationCountry { get; set; }
        public DateTime? DepartureDate { get; set; }
        public string? DepartureAirport { get; set; }
        public string? ArrivalAirport { get; set; }
        public int? Passenger { get; set; }
        public int? FlightClass { get; set; }
    }

}