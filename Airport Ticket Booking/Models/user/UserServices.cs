namespace Airport_Ticket_Booking.Models.user;

public class UserServices : IUserServices
{
    private readonly IUserRepository _userRepository;

    public UserServices(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }


    public User? SignIn(string username, string password)
    {
        User user = new User(username, password);
        return _userRepository.Authentication(user);
    }

    public bool SignUp(string username, string password)
    {
        User user = new User(username, password);
        return _userRepository.Create(user);
    }
}