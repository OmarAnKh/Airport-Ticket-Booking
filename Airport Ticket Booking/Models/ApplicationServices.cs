using Airport_Ticket_Booking.Models.flight;
using Airport_Ticket_Booking.Models.user;

namespace Airport_Ticket_Booking.Models
{
    public class ApplicationServices
    {
        private readonly IFlightServices _flightServices;
        private readonly IUserServices _userServices;
        private User? _user;
        private readonly static Lock Lock = new Lock();
        private static ApplicationServices? _instance;

        private ApplicationServices()
        {
            _flightServices = new FlightServices("../../../Data/flight.txt");
            _userServices = new UserServices("../../../Data/users.txt");

        }

        public static ApplicationServices? GetInstance()
        {
            lock (Lock)
            {
                _instance ??= new ApplicationServices();
            }
            return _instance;
        }
        public bool SignIn(string username, string password)
        {
            User? user = _userServices.SignIn(username, password);
            if (user != null)
            {
                _user = user;
                return true;
            }
            return false;
        }
        public int Book(int flightId)
        {
            if (_user == null)
            {
                return 1;
            }
            
            return _flightServices.BookFlight(flightId,_user.UserId) ? 0 : 2;
        }


    }
}