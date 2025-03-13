using Airport_Ticket_Booking.Models.flight;
using AutoFixture;

namespace Airport_Ticket_Booking.Test
{
    public class FlightFilterShould
    {
        private readonly FlightFilterService _flightFilterService = new();
        private readonly Fixture _fixture = new();

        public FlightFilterShould()
        {
            _fixture.Inject(DateTime.UtcNow.Add(new TimeSpan(10000)));
            _fixture.Inject(1000);
            _fixture.Inject(false);
        }
        [Fact]
        public void FilterFlightsCorrectly()
        {
            //Arrange
            var flights=_fixture.CreateMany<Flight>(10).ToList();
            var departureDate=flights.First().DepartureDate;
            var departureCountry=flights.First().DepartureCountry;
            var destinationCountry=flights.First().DestinationCountry;
            
            //Act
            var result = _flightFilterService.FilterFlights(flights, departureDate: departureDate, departureCountry:departureCountry, destinationCountry:destinationCountry);
            
            //Assert
            Assert.NotEmpty(result);
        }
    }
}