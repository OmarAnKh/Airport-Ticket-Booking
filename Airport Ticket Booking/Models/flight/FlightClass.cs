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
        { (FlightClass.Business, FlightClass.First), 1.5m }
    };

    public static decimal CalculateFlightPrice(this FlightClass flightClass, decimal currentPrice, FlightClass targetFlightClass)
    {
        if (flightClass == targetFlightClass)
            return currentPrice; 

        if (ClassMultipliers.TryGetValue((flightClass, targetFlightClass), out decimal multiplier))
        {
            return currentPrice * multiplier;
        }

        if (ClassMultipliers.TryGetValue((targetFlightClass, flightClass), out decimal reverseMultiplier))
        {
            return currentPrice / reverseMultiplier;
        }

        throw new InvalidOperationException($"Unsupported class transition from {flightClass} to {targetFlightClass}.");
    }
}