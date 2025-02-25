using System.Globalization;
using Airport_Ticket_Booking.Models.flight;
using Airport_Ticket_Booking.Models.user;

namespace Airport_Ticket_Booking.Models
{
    public delegate void PrintMenuDelegate();

    public class ApplicationServices
    {
        private readonly IFlightServices _flightServices;
        private readonly IUserServices _userServices;
        private User? _user;

        private static readonly Lock Lock = new();
        private static ApplicationServices? _instance;

        private ApplicationServices(IFlightServices flightServices, IUserServices userServices)
        {
            _flightServices = flightServices;
            _userServices = userServices;
        }

        public static ApplicationServices GetInstance(IFlightServices flightServices, IUserServices userServices)
        {
            lock (Lock)
            {
                _instance ??= new ApplicationServices(flightServices, userServices);
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

            Console.WriteLine("Invalid username or password.");
            return false;
        }

        public bool SignUp(string username, string password)
        {
            return _userServices.SignUp(username, password);
        }

        private int Book(int flightId)
        {
            if (_user == null)
            {
                return 1;
            }

            return _flightServices.BookFlight(flightId, _user.UserId) ? 0 : 2;
        }

        private int ModifyFlightClass(int flightId, int flightClassId)
        {
            if (_user == null)
            {
                return 1;
            }

            return _flightServices.ModifyFlight(flightId, flightClassId, _user.UserId) ? 0 : 2;
        }

        private int Cancel(int flightId)
        {
            if (_user == null)
            {
                return 1;
            }

            return _flightServices.CancelFlight(flightId) ? 0 : 2;
        }

        private void SearchFlights(string? departureCountry = null,
            string? destinationCountry = null, DateTime? departureDate = null,
            string? departureAirport = null, string? arrivalAirport = null, string? flightClass = null,
            decimal? maxPrice = null)
        {
            _flightServices.SearchFlights(departureCountry, destinationCountry,
                departureDate, departureAirport, arrivalAirport, flightClass, maxPrice);
        }

        private bool ShowMyFlights()
        {
            if (_user == null)
            {
                return false;
            }

            _flightServices.ShowMyFlights(_user.UserId);
            return true;
        }

        private void FilterBookings(int? flightId = null, decimal? price = null, string? departureCountry = null,
            string? destinationCountry = null, DateTime? departureDate = null, string? departureAirport = null,
            string? arrivalAirport = null, int? passenger = null, int flightClass = 0)
        {
            if (_user == null)
            {
                Console.WriteLine("You need to sign in before filtering bookings");
                return;
            }

            if ((int)_user.Role == 0)
            {
                Console.WriteLine("Your must be a Manager to Filter the Booking");
                return;
            }

            var bookings = _flightServices.GetAllFlights();

            var filteredBookings = bookings.Where(b =>
                (b.FlightId == flightId) ||
                (b.Price == price) ||
                (b.DepartureCountry == departureCountry) ||
                (b.DestinationCountry == destinationCountry) ||
                (b.DepartureDate == departureDate) ||
                (b.DepartureAirport == departureAirport) ||
                (b.ArrivalAirport == arrivalAirport) ||
                (b.PassengerId == passenger) ||
                (b.Class == (FlightClass)flightClass)
            ).ToList();
            _flightServices.DisplayFlights(filteredBookings);
        }

        private List<string>? ImportFlights(string flightsCsv)
        {
            if (_user == null)
            {
                Console.WriteLine("You need to sign in before filtering bookings");
                return null;
            }

            if ((int)_user.Role == 0)
            {
                Console.WriteLine("Your must be a Manager to Filter the Booking");
                return null;
            }

            return _flightServices.ImportFlightsFromCsv(flightsCsv);
        }

        public void SignInMenu()
        {
            Console.WriteLine("1) Sign In");
            Console.WriteLine("2) Sign Up");
            Console.WriteLine("3) Exit");
        }

        public void PrintMenu()
        {
            if (_user == null)
            {
                Console.WriteLine("You need to sign in before accessing the menu.");
                return;
            }

            bool isManager = (int)_user.Role != 0;

            Console.WriteLine("1) Search for Flights");
            Console.WriteLine("2) Show My Flights");
            Console.WriteLine("3) Book A Flight");
            Console.WriteLine("4) Modify A Booking");
            Console.WriteLine("5) Cancel Booking");

            if (isManager)
            {
                Console.WriteLine("6) Filter Flights");
                Console.WriteLine("7) Import Flights");
            }

            Console.Write("Enter your choice: ");
            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                HandleMenuChoice(choice, isManager);
            }
            else
            {
                Console.WriteLine("Invalid input.");
            }
        }

        private void HandleMenuChoice(int choice, bool isManager)
        {
            switch (choice)
            {
                case (int)MainOptions.Search:
                    var searchCriteria = GetFlightSearchCriteria();
                    SearchFlights(
                        departureCountry: searchCriteria.departureCountry,
                        destinationCountry: searchCriteria.destinationCountry,
                        departureDate: searchCriteria.departureDate,
                        departureAirport: searchCriteria.departureAirport,
                        arrivalAirport: searchCriteria.arrivalAirport,
                        flightClass: searchCriteria.flightClass,
                        maxPrice: searchCriteria.maxPrice
                    );
                    break;
                case (int)MainOptions.ShowMyFlights:
                    if (!ShowMyFlights())
                        Console.WriteLine("You have no booked flights.");
                    break;
                case (int)MainOptions.Book:
                    Console.Write("Enter Flight ID to book: ");
                    if (int.TryParse(Console.ReadLine(), out int flightId))
                    {
                        Console.WriteLine(Book(flightId) == 0 ? "Booking successful." : "Booking failed.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid Flight ID.");
                    }

                    break;
                case (int)MainOptions.Modify:
                    Console.Write("Enter Flight ID to modify: ");
                    if (int.TryParse(Console.ReadLine(), out int modFlightId))
                    {
                        Console.Write("Flight Class (0 for Economy, 1 for Business, etc.): ");
                        if (int.TryParse(Console.ReadLine(), out int flightClassId))
                        {
                            Console.WriteLine(ModifyFlightClass(modFlightId, flightClassId) == 0
                                ? "Modification successful."
                                : "Modification failed.");
                        }
                    }

                    break;
                case (int)MainOptions.Cancel:
                    Console.Write("Enter Flight ID to cancel: ");
                    if (int.TryParse(Console.ReadLine(), out int cancelFlightId))
                    {
                        Console.WriteLine(Cancel(cancelFlightId) == 0
                            ? "Cancellation successful."
                            : "Cancellation failed.");
                    }

                    break;
                case (int)MainOptions.Filter when isManager:
                    var filterCriteria = GetFlightFilterCriteria();
                    FilterBookings(
                        flightId: filterCriteria.flightId,
                        price: filterCriteria.price,
                        departureCountry: filterCriteria.departureCountry,
                        destinationCountry: filterCriteria.destinationCountry,
                        departureDate: filterCriteria.departureDate,
                        departureAirport: filterCriteria.departureAirport,
                        arrivalAirport: filterCriteria.arrivalAirport,
                        passenger: filterCriteria.passenger,
                        flightClass: filterCriteria.flightClass
                    );
                    break;
                case (int)MainOptions.ImportFlights when isManager:
                    Console.Write("Enter CSV file path to import flights: ");
                    string? csvPath = Console.ReadLine();
                    var errors = ImportFlights(csvPath!);
                    if (errors != null)
                    {
                        foreach (var error in errors)
                        {
                            Console.WriteLine(error);
                        }
                    }

                    break;
                default:
                    Console.WriteLine("Invalid choice. Try again.");
                    break;
            }
        }


        static (int? flightId, decimal? price, string? departureCountry, string? destinationCountry, DateTime?
            departureDate, string? departureAirport, string? arrivalAirport, int? passenger, int flightClass)
            GetFlightFilterCriteria()
        {
            Console.Write("Flight ID: ");
            int? flightId = TryParseInt(Console.ReadLine());

            Console.Write("Price: ");
            decimal? price = TryParseDecimal(Console.ReadLine());

            Console.Write("Departure Country: ");
            string? departureCountry = Console.ReadLine();

            Console.Write("Destination Country: ");
            string? destinationCountry = Console.ReadLine();

            Console.Write("Departure Date (yyyy-MM-dd): ");
            DateTime? departureDate = TryParseDate(Console.ReadLine());

            Console.Write("Departure Airport: ");
            string? departureAirport = Console.ReadLine();

            Console.Write("Arrival Airport: ");
            string? arrivalAirport = Console.ReadLine();

            Console.Write("Passenger ID: ");
            int? passenger = TryParseInt(Console.ReadLine());

            Console.Write("Flight Class (0 for Economy, 1 for Business, 2 for First): ");
            int flightClass = TryParseInt(Console.ReadLine()) ?? 0;

            return (flightId, price, departureCountry, destinationCountry, departureDate, departureAirport,
                arrivalAirport, passenger, flightClass);
        }


        static (string? departureCountry, string? destinationCountry, DateTime? departureDate, string? departureAirport,
            string? arrivalAirport, string? flightClass, decimal? maxPrice) GetFlightSearchCriteria()
        {
            Console.Write("Departure Country: ");
            string? departureCountry = Console.ReadLine();

            Console.Write("Destination Country: ");
            string? destinationCountry = Console.ReadLine();

            Console.Write("Departure Date (yyyy-MM-dd): ");
            DateTime? departureDate = TryParseDate(Console.ReadLine());

            Console.Write("Departure Airport: ");
            string? departureAirport = Console.ReadLine();

            Console.Write("Arrival Airport: ");
            string? arrivalAirport = Console.ReadLine();

            Console.Write("Flight Class: ");
            string? flightClass = Console.ReadLine();

            Console.Write("Max Price: ");
            decimal? maxPrice = TryParseDecimal(Console.ReadLine());

            return (departureCountry, destinationCountry, departureDate, departureAirport, arrivalAirport, flightClass,
                maxPrice);
        }

        static int? TryParseInt(string? input) => int.TryParse(input, out int value) ? value : null;

        static decimal? TryParseDecimal(string? input) =>
            decimal.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal value) ? value : null;

        static DateTime? TryParseDate(string? input) => DateTime.TryParseExact(input, "yyyy-MM-dd",
            CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date)
            ? date
            : null;
    }
}