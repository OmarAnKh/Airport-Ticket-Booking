namespace Airport_Ticket_Booking.Models.user;

public class UserServices : IUserServices
{
    private readonly IUserRepository _userRepository;

    public UserServices(string path)
    {
        _userRepository = new UserRepository(path);
    }


    public bool SignIn(string username, string password)
    {
        var user = new User(username, password);
        return _userRepository.Authentication(user);
    }

    public void SignUp(string username, string password)
    {
        var user = new User(username, password);
        _userRepository.Create(user);
        
    }

    public bool Authorize(string username, string password)
    {
        throw new NotImplementedException();
    }
}