using Airport_Ticket_Booking.Models.flight;
using AutoFixture;
using FluentAssertions;


namespace Airport_Ticket_Booking.Test;

public class FlightSearchShould
{
    private readonly FlightSearchService _flightSearchService = new();
    private readonly Fixture _fixture = new();

    public FlightSearchShould()
    {
           var random = new Random();
                _fixture.Customize<Flight>(flight => flight
                    .With(f => f.DepartureDate, DateTime.UtcNow.AddDays(random.Next(1, 31)))
                    .With(f => f.Price, random.Next(100, 2000))
                    .With(f => f.IsBook, false));
    }
    [Fact]
    public void SearchFlightsCorrectly()
    {
        // Arrange
        var flights = _fixture.CreateMany<Flight>(10).ToList();
        var departureDate = flights.First().DepartureDate;
        var departureCountry = flights.First().DepartureCountry;
        var destinationCountry = flights.First().DestinationCountry;

        // Act
        var result = _flightSearchService.SearchFlights(flights:flights,departureDate:departureDate ,departureCountry:departureCountry,destinationCountry:destinationCountry);

        // Assert
        result.Should().NotBeEmpty();
    }
}