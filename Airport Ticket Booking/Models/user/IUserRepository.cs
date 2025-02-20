namespace Airport_Ticket_Booking.Models.user;

public interface IUserRepository
{
    public List<User> GetAllData();
    public bool Create(User user);
    public bool Authentication(User user);

}