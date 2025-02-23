namespace Airport_Ticket_Booking.Models.user;

public interface IUserServices
{
    public User? SignIn(string username, string password);
    public bool SignUp(string username, string password);
    
}