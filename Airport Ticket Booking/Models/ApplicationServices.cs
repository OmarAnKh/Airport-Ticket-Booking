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

        public int ModifyFlightClass(int flightId, int flightClassId)
        {
            if (_user == null)
            {
                return 1;
            }
            
            return _flightServices.ModifyFlight(flightId, flightClassId, _user.UserId) ? 0 : 2;
        }

        public int Cancel(int flightId)
        {
            if (_user == null)
            {
                return 1;
            }
            return _flightServices.CancelFlight(flightId) ? 0 : 2;
        }

        public bool ShowMyFlights()
        {
            if (_user == null)
            {
                return false;
            }
            _flightServices.ShowMyFlights(_user.UserId);
            return true;
        }
        public List<Flight> FilterBookings(int? flightId = null, decimal? price = null, string? departureCountry = null, 
            string? destinationCountry = null, DateTime? departureDate = null, string? departureAirport = null, 
            string? arrivalAirport = null, int? passenger = null, FlightClass? flightClass = null)
        {
            var bookings = _flightServices.GetAllFlights(); 

            var filteredBookings = bookings.Where(b =>
                (flightId == null || b.FlightId == flightId) &&
                (price == null || b.Price == price) &&
                (departureCountry == null || b.DepartureCountry == departureCountry) &&
                (destinationCountry == null || b.DestinationCountry == destinationCountry) &&
                (departureDate == null || b.DepartureDate.Date == departureDate.Value.Date) &&
                (departureAirport == null || b.DepartureAirport == departureAirport) &&
                (arrivalAirport == null || b.ArrivalAirport == arrivalAirport) &&
                (passenger == null || b.PassengerId == passenger) &&
                (flightClass == null || b.Class == flightClass)
            ).ToList();
            return filteredBookings;
        }

    }
}