using Airport_Ticket_Booking.Models.flight;
using AutoFixture;
using FluentAssertions;

namespace Airport_Ticket_Booking.Test;
public class BookingManagerShould
{
    private readonly Fixture _fixture;
    private readonly BookingManager _bookingManager;

    public BookingManagerShould()
    {
        _fixture = new Fixture();
        _bookingManager = new BookingManager();

        var random = new Random();
        _fixture.Customize<Flight>(flight => flight
            .With(f => f.DepartureDate, DateTime.UtcNow.AddDays(random.Next(1, 31)))
            .With(f => f.Price, random.Next(100, 2000)));
    }
    

    [Theory]
    [InlineData(false, 123, true)]
    [InlineData(true, 123, false)]
    [InlineData(true, null, false)]
    public void ShouldBookFlightBasedOnBookingStateAndPassengerId(bool isBook, int passengerId, bool expectedBookingResult)
    {
        // Arrange
        var flights = _fixture.CreateMany<Flight>(5).ToList();
        var flight = flights[0];
        flight.IsBook = isBook;

        // Act
        var result = _bookingManager.Book(flights, flight.FlightId, passengerId);

        // Assert
        result.Should().Be(expectedBookingResult);
    }
    
    
    
    [Theory]
    [InlineData(true, true)]
    [InlineData(false, false)]
    public void ShouldCancelFlightBasedOnBookingStateAndPassengerId(bool isBook, bool expectedCancellationResult)
    {
        // Arrange
        var flights = _fixture.CreateMany<Flight>(5).ToList();

        var flight = new Flight(
            departureDate: DateTime.Today.AddDays(3), 
            price: 1000, 
            departureCountry: "USA",
            destinationCountry: "UK",
            departureAirport: "JFK",
            arrivalAirport: "LHR",
            @class: FlightClass.Business,
            isBook: isBook,
            passengerId: 0,
            flightId: 1
        )
        {
            DepartureCountry = "USA",
            DepartureAirport = "JFK",
            DestinationCountry = "UK",
            ArrivalAirport = "LHR"
        };

        flights.Add(flight);

        // Act
        var result = _bookingManager.Cancel(flights, flight.FlightId);

        // Assert
        result.Should().Be(expectedCancellationResult);
    }
    
    [Theory]
    [InlineData(FlightClass.Economy, FlightClass.Business, 200, 300, true)]
    [InlineData(FlightClass.Business, FlightClass.First, 500, 750, true)] 
    [InlineData(FlightClass.First, FlightClass.Economy, 700, 350, true)] 
    public void ModifyClassShouldReturnTrueWhenClassChanges(FlightClass initialClass, FlightClass targetClass, decimal initialPrice, decimal expectedPrice, bool expectedResult)
    {
        // Arrange
        var flights = _fixture.CreateMany<Flight>(5).ToList();
        var flight = flights[0];
        flight.IsBook = true;
        flight.PassengerId = 123;
        flight.Class = initialClass;
        flight.Price = initialPrice;

        // Act
        decimal newPrice = flight.Class.CalculateFlightPrice(initialPrice, targetClass);
        flight.Class = targetClass;
        flight.Price = newPrice;

        // Assert
        Assert.Equal(targetClass, flight.Class);
        Assert.Equal(expectedPrice, flight.Price);
        Assert.True(expectedResult);
    }
    
}

