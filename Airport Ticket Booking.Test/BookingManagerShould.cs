using Airport_Ticket_Booking.Models.flight;
using AutoFixture;
using FluentAssertions;

namespace Airport_Ticket_Booking.Test
{
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

        [Fact]
        public void ShouldBookFlightSuccessfully()
        {
            // Arrange
            var flights = _fixture.CreateMany<Flight>(5).ToList();
            var flight = flights[0];
            flight.IsBook = false;

            // Act
            var result = _bookingManager.Book(flights, flight.FlightId, 123);

            // Assert
            result.Should().BeTrue("because the flight was available for booking");
            flight.IsBook.Should().BeTrue("because the flight should now be booked");
            flight.PassengerId.Should().Be(123, "because the provided passenger ID should be assigned to the flight");
        }

        [Fact]
        public void Book_ShouldReturnFalse_WhenFlightIsAlreadyBooked()
        {
            // Arrange
            var flights = _fixture.CreateMany<Flight>(5).ToList();
            var flight = flights[0];
            flight.IsBook = true;

            // Act
            var result = _bookingManager.Book(flights, flight.FlightId, 123);

            // Assert
            result.Should().BeFalse("because the flight is already booked");
        }

        [Theory]
        [InlineData(false, 123, true)]
        [InlineData(true, 123, false)]
        [InlineData(true, null, false)]
        public void BookAFlightShould(bool isBook, int passengerId, bool expectedBookingResult)
        {
            // Arrange
            var flights = _fixture.CreateMany<Flight>(5).ToList();
            var flight = flights[0];
            flight.IsBook = isBook;

            // Act
            var result = _bookingManager.Book(flights, flight.FlightId, passengerId);

            // Assert
            result.Should().Be(expectedBookingResult, $"because the flight booking state was {isBook} and the passenger ID was {passengerId}");
        }
        
        
        
        [Fact]
        public void CancelShouldReturnTrueWhenFlightIsBooked()
        {
            // Arrange
            var flights = _fixture.CreateMany<Flight>(5).ToList();
            var flight = flights[0];
            flight.IsBook = true;

            // Act
            var result = _bookingManager.Cancel(flights, flight.FlightId);

            // Assert
            Assert.True(result);
            Assert.False(flight.IsBook);
        }
        
        [Fact]
        public void CancelShouldReturnFalseWhenFlightIsNotBooked()
        {
            // Arrange
            var flights = _fixture.CreateMany<Flight>(5).ToList();
            var flight = flights[0];
            flight.IsBook = false;

            // Act
            var result = _bookingManager.Cancel(flights, flight.FlightId);

            // Assert
            Assert.False(result);
        }

        
        [Fact]
        public void ModifyClassShouldReturnTrueWhenClassChanges()
        {
            // Arrange
            var flights = _fixture.CreateMany<Flight>(5).ToList();
            var flight = flights[0];
            flight.IsBook = true;
            flight.PassengerId = 123;
            flight.Class = FlightClass.Economy;
            flight.Price = 200;

            var currentPrice = flight.Price;
            var targetClass = FlightClass.Business;

            // Act
            decimal newPrice = flight.Class.CalculateFlightPrice(currentPrice, targetClass);
            flight.Class = targetClass;
            flight.Price = newPrice;

            // Assert
            Assert.True(flight.Class == targetClass);
            Assert.Equal(300, flight.Price);
        }

        [Fact]
        public void ModifyClassShouldReturnFalseWhenUserIsNotPassenger()
        {
            // Arrange
            var flights = _fixture.CreateMany<Flight>(5).ToList();
            var flight = flights[0];
            flight.IsBook = true;
            flight.PassengerId = 123;
            flight.Class = FlightClass.Economy;

            // Act
            var result = _bookingManager.ModifyClass(flights, flight.FlightId, (int)FlightClass.Business, 456);

            // Assert
            Assert.False(result);
        }
        
    }
}
