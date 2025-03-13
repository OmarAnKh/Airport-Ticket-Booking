using Airport_Ticket_Booking.Models.flight;
using AutoFixture;


namespace Airport_Ticket_Booking.Test;

public class FlightSearchShould
{
    private readonly FlightSearchService _flightSearchService = new();
    private readonly Fixture _fixture = new();

    public FlightSearchShould()
    {
        _fixture.Inject(DateTime.UtcNow.Add(new TimeSpan(10000)));
        _fixture.Inject(1000);
        _fixture.Inject(false);
    }

    [Fact]
    public void SearchFlightsCorrectly()
    {
        // Arrange

        // _fixture.Customize<Flight>(flight => flight
        //     .With(date => date.DepartureDate, DateTime.UtcNow.AddDays(1))
        //     .With(price => price.Price, 1000)
        //     .With(status => status.IsBook, false));
        
        var flights = _fixture.CreateMany<Flight>(10).ToList();
        var departureDate = flights.First().DepartureDate;
        var departureCountry = flights.First().DepartureCountry;
        var destinationCountry = flights.First().DestinationCountry;

        // Act
        var result = _flightSearchService.SearchFlights(flights:flights,departureDate:departureDate ,departureCountry:departureCountry,destinationCountry:destinationCountry);

        // Assert
        Assert.Single(result);
    }
}