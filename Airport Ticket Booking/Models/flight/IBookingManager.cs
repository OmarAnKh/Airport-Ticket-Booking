namespace Airport_Ticket_Booking.Models.flight;

public interface IBookingManager
{
    public bool Book();
    public bool Cancel();
    public bool Modify();
}