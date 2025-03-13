using Airport_Ticket_Booking.Models.flight;
using AutoFixture;


namespace Airport_Ticket_Booking.Test;

public class FlightSearchShould
{
    private readonly FlightSearchService _flightSearchService = new();
    private readonly Fixture _fixture = new();

    public FlightSearchShould()
    {
        _fixture.Inject<DateTime>(DateTime.UtcNow.Add(new TimeSpan(10000)));
        _fixture.Inject<int>(1000);
        _fixture.Inject<bool>(false);

        
    }

    [Fact]
    public void Test1()
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
        var result = _flightSearchService.SearchFlights(flights, departureCountry, destinationCountry, departureDate);

        // Assert
        Assert.NotEmpty(result);
    }
}