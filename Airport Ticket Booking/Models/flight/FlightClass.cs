namespace Airport_Ticket_Booking.Models.flight;

public enum FlightClass
{
    Economy,
    Business,
    First
}

public static class FlightClassExtensions
{

    private readonly static Dictionary<(FlightClass, FlightClass), decimal> ClassMultipliers = new Dictionary<(FlightClass, FlightClass), decimal>
    {
        { (FlightClass.Economy, FlightClass.Business), 1.5m },
        { (FlightClass.Economy, FlightClass.First), 2m },
        { (FlightClass.Business, FlightClass.Economy), (decimal)(1 / 1.5) },
        { (FlightClass.Business, FlightClass.First), 1.5m },
        { (FlightClass.First, FlightClass.Economy), 0.5M},
        { (FlightClass.First, FlightClass.Business), (decimal)(1 / 1.5) }
    };

    public static decimal CalculateFlightPrice(this FlightClass flightClass, decimal currentPrice, FlightClass targetFlightClass)
    {
        decimal calculatedPrice = currentPrice;
        if (ClassMultipliers.TryGetValue((flightClass, targetFlightClass), out decimal flightMultiplier))
        {
            calculatedPrice  *= flightMultiplier;
            return calculatedPrice;
        }
        throw new InvalidOperationException("Unsupported class transition.");
    }
}