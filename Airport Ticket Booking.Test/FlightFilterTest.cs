using Airport_Ticket_Booking.Models.flight;
using Airport_Ticket_Booking.Models.flight.Services;
using AutoFixture;
using FluentAssertions;

namespace Airport_Ticket_Booking.Test;
public class FlightFilterTest
{
    private readonly FlightFilterService _flightFilterService = new();
    private readonly Fixture _fixture = new();

    public FlightFilterTest()
    {
        var random = new Random();
        _fixture.Customize<Flight>(flight => flight
            .With(f => f.DepartureDate, DateTime.UtcNow.AddDays(random.Next(1, 31)))
            .With(f => f.Price, random.Next(100, 2000))
            .With(f => f.IsBook, false));
    }
    [Fact]
    public void FilterFlights_ShouldReturnNonEmptyList_WhenValidDepartureAndDestinationAreGiven()
    {
        // Arrange
        var flights = _fixture.CreateMany<Flight>(10).ToList();
        var departureDate = flights.First().DepartureDate;
        var departureCountry = flights.First().DepartureCountry;
        var destinationCountry = flights.First().DestinationCountry;

        var filterCriteria = new FlightFilterCriteria
        {
            DepartureDate = departureDate,
            DepartureCountry = departureCountry,
            DestinationCountry = destinationCountry
        };

        // Act
        var result = _flightFilterService.FilterFlights(flights, filterCriteria);

        // Assert
        result.Should().NotBeEmpty();
    }

}
