using Airport_Ticket_Booking.Models.flight;
using Airport_Ticket_Booking.Models.flight.Services;
using AutoFixture;
using FluentAssertions;


namespace Airport_Ticket_Booking.Test;

public class FlightSearchTest
{
    private readonly FlightSearchService _flightSearchService = new();
    private readonly Fixture _fixture = new();

    public FlightSearchTest()
    {
           var random = new Random();
                _fixture.Customize<Flight>(flight => flight
                    .With(f => f.DepartureDate, DateTime.UtcNow.AddDays(random.Next(1, 31)))
                    .With(f => f.Price, random.Next(100, 2000))
                    .With(f => f.IsBook, false));
    }
    [Fact]
    public void SearchFlights_ShouldReturnNonEmptyList_WhenValidSearchCriteriaAreGiven()
    {
        // Arrange
        var flights = _fixture.CreateMany<Flight>(10).ToList();
        var departureDate = flights.First().DepartureDate;
        var departureCountry = flights.First().DepartureCountry;
        var destinationCountry = flights.First().DestinationCountry;

        var searchCriteria = new FlightSearchCriteria
        {
            DepartureDate = departureDate,
            DepartureCountry = departureCountry,
            DestinationCountry = destinationCountry
        };

        // Act
        var result = _flightSearchService.SearchFlights(flights, searchCriteria);

        // Assert
        result.Should().NotBeEmpty();
    }

}