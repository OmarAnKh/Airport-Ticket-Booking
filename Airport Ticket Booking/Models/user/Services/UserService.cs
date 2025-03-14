using Airport_Ticket_Booking.Models.user.Repositories.Interfaces;

namespace Airport_Ticket_Booking.Models.user.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public List<User> GetAllUsers() => _userRepository.GetAllData();

    public bool RegisterUser(string username, string password)
    {
        var existingUser = _userRepository.Authentication(username);
        if (existingUser != null) return false;

        int userId = GetNextUserId();
        string hashedPassword = HashPassword(password);
        var newUser = new User(username, hashedPassword, UserRole.Passenger, userId);

        return _userRepository.Create(newUser);
    }

    public User? AuthenticateUser(string username, string password)
    {
        var user = _userRepository.Authentication(username);
        if (user == null || !VerifyPassword(password, user.Password))
            return null;
        
        return user;
    }

    private static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private static bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
    
    private int GetNextUserId()
    {
        var users = _userRepository.GetAllData();
        return users.Count != 0 ? users.Max(u => u.UserId) + 1 : 1;
    }
}


