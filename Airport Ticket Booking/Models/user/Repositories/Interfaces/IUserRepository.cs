namespace Airport_Ticket_Booking.Models.user.Repositories.Interfaces;

public interface IUserRepository
{
    public List<User> GetAllData();
    public bool Create(User user);
    public User? Authentication(string username);

}