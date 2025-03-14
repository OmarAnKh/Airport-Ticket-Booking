namespace Airport_Ticket_Booking.Models.user.Repositories.Interfaces;

public interface IUserService
{
    public User? AuthenticateUser(string username, string password);
    public bool RegisterUser(string username, string password);
    
}