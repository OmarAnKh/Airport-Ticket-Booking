namespace Airport_Ticket_Booking.Models.user;

public class UserRepository : IUserRepository
{
    private readonly string? _fileString;
    private readonly List<User?> _list;

    public UserRepository(string? fileString)
    {
        _fileString = fileString;
        _list = [];
        _list = GetAllData();
    }

    public List<User?> GetAllData()
    {
        try
        {
            if (_list.Count > 0) return _list;
            _list.AddRange(File.ReadAllLines(_fileString!).Select(line => line.Split(","))
                .Select(data => new User(data[1], data[2], data[3], int.Parse(data[0]))));
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }

        return _list;
    }

    public User? GetById(int id)
    {
        User? user;
        try
        {
            user = _list.SingleOrDefault(userData => userData?.UserId == id);
        }
        catch (Exception e)
        {
            throw new Exception($"More than one user with this id found, {e.Message}");
        }

        return user;
    }


    public void Create(User user)
    {
        try
        {
            var userId = _list.Count != 0 ? _list.Max(u => u!.UserId) + 1 : 1;
            var hashedPassword = HashPassword(user.Password);
            File.AppendAllText(_fileString!, $"{userId},{user.Username},{hashedPassword},{user.Role}\n");
            _list.Add(new User(user.Username, hashedPassword, user.Role, userId));
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public bool Authontication(User user)
    {
        try
        {
            var exist = _list.Single(data => data?.Username == user?.Username);
            if (exist?.Role != user.Role)
            {
                return false;
            }

            return VerifyPassword(user.Password, exist.Password);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}