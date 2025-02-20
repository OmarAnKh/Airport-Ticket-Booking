namespace Airport_Ticket_Booking.Models.user;

public interface IUserServices
{
    public bool SignIn(string username, string password);
    public bool SignUp(string username, string password);
    public bool Authorize(string username, string password);
}