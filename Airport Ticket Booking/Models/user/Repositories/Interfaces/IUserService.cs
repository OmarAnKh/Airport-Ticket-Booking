namespace Airport_Ticket_Booking.Models.user.Repositories.Interfaces;

public interface IUserService
{
    User? AuthenticateUser(string username, string password);
    bool RegisterUser(string username, string password);
    
}