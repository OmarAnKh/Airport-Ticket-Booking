namespace Airport_Ticket_Booking.Models.user;

public interface IUserRepository
{
    public List<User?> GetAllData();
    public User? GetById(int id);
    public void Create(User user);

}