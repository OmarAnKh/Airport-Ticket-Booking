namespace Airport_Ticket_Booking.Models.user.Repositories.Interfaces;

public interface IUserRepository
{
    List<User> GetAllData();
    bool Create(User user);
    User? Authentication(string username);

}