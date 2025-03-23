namespace Airport_Ticket_Booking.Models.flight.Services
{
    public class FlightSearchCriteria
    {
        public string? DepartureCountry { get; set; }
        public string? DestinationCountry { get; set; }
        public DateTime? DepartureDate { get; set; }
        public string? DepartureAirport { get; set; }
        public string? ArrivalAirport { get; set; }
        public FlightClass? FlightClass { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}