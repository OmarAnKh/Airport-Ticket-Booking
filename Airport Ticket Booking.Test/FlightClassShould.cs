using Airport_Ticket_Booking.Models.flight;
using FluentAssertions;

namespace Airport_Ticket_Booking.Test
{
    public class FlightClassShould
    {
        public static IEnumerable<object[]> FlightPriceTestData()
        {
            yield return [100m, FlightClass.Economy, FlightClass.Business, 150m];
            yield return [100m, FlightClass.Economy, FlightClass.First, 200m];
            yield return [100m, FlightClass.Business, FlightClass.Economy, 66.67m];
            yield return [100m, FlightClass.Business, FlightClass.First, 150m];
            yield return [100m, FlightClass.First, FlightClass.Economy, 50m];
            yield return [100m, FlightClass.First, FlightClass.Business, 66.67m];
        }
        
        [Theory]
        [MemberData(nameof(FlightPriceTestData))]
        public void CalculateFlightPriceShouldReturnExpectedPrice(decimal currentPrice, FlightClass flightClass, FlightClass targetFlightClass, decimal expectedPrice)
        {
            // Act
            decimal result = flightClass.CalculateFlightPrice(currentPrice, targetFlightClass);

            // Assert
            result.Should().BeApproximately(expectedPrice, 0.01m); 
        }
        
        [Fact]
        public void CalculateFlightPriceShouldThrowInvalidOperationException_WhenClassTransitionIsUnsupported()
        {
            // Arrange
            decimal currentPrice = 100m;
            FlightClass flightClass = FlightClass.Economy;
            FlightClass targetFlightClass = FlightClass.Economy;

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => flightClass.CalculateFlightPrice(currentPrice, targetFlightClass));
        }
    }
}