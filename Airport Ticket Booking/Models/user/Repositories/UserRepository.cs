using Airport_Ticket_Booking.Models.user.Repositories.Interfaces;

namespace Airport_Ticket_Booking.Models.user.Repositories; 
public class UserRepository : IUserRepository
{
    private static UserRepository? _instance;
    private readonly static object Lock = new();
    private readonly string? _fileString;
    private readonly List<User> _users;
    
    private UserRepository(string fileString)
    {
        _fileString = fileString;
        _users = new List<User>();
    }

    public static UserRepository GetInstance(string fileString)
    {
        if (_instance == null)
        {
            lock (Lock)
            {
                _instance ??= new UserRepository(fileString);
            }
        }
        return _instance;
    }

    public List<User> GetAllData()
    {
        try
        {
            if (_users.Count > 0) return _users;
            _users.AddRange(File.ReadAllLines(_fileString!).Select(line => line.Split(","))
                .Select(data => new User(data[1], data[2], Enum.Parse<UserRole>(data[3]), int.Parse(data[0]))));
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }

        return _users;
    }

    public bool Create(User user)
    {
        try
        {
            User? usernameExist = _users.SingleOrDefault(u => u.Username == user.Username);
            if (usernameExist != null)
            {
                return false;
            }
            File.AppendAllText(_fileString!, $"{user.UserId},{user.Username},{user.Password},{user.Role}\n");
            _users.Add(user);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        return true;
    }

    public User? Authentication(string username) => _users.SingleOrDefault(u => u.Username == username);
}