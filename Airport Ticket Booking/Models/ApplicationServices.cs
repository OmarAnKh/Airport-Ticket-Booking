using System.Globalization;
using Airport_Ticket_Booking.Models.flight;
using Airport_Ticket_Booking.Models.flight.Services;
using Airport_Ticket_Booking.Models.flight.Services.interfaces;
using Airport_Ticket_Booking.Models.UI;
using Airport_Ticket_Booking.Models.user;
using Airport_Ticket_Booking.Models.user.Repositories.Interfaces;

namespace Airport_Ticket_Booking.Models;

public class ApplicationServices
{
   private readonly IFlightService _flightService;
    private readonly IUserService _userService;
    private readonly IUiService _uiService;
    private User? _userBackingField;

    public User User
    {
        get
        {
            if (_userBackingField == null)
            {
                throw new InvalidOperationException("User is not authenticated.");
            }
            return _userBackingField;
        }
    }

    private static readonly Lock Lock = new();
    private static ApplicationServices? _instance;

    private ApplicationServices(IFlightService flightService, IUserService userService, IUiService uiService)
    {
        _flightService = flightService;
        _userService = userService;
        _uiService = uiService;
    }

    public static ApplicationServices GetInstance(IFlightService flightService, IUserService userService, IUiService uiService)
    {
        lock (Lock)
        {
            _instance ??= new ApplicationServices(flightService, userService, uiService);
        }

        return _instance;
    }

    public ReturnValues SignIn(string username, string password)
    {
        var user = _userService.AuthenticateUser(username, password);
        if (user != null)
        {
            _userBackingField = user;
            return ReturnValues.Succeed;
        }

        Console.WriteLine("Invalid username or password.");
        return ReturnValues.Failure;
    }

    public bool SignUp(string username, string password)
    {
        if (_userService.RegisterUser(username, password))
        {
            SignIn(username, password);
            return true;
        }
        return false;
    }

    private ReturnValues Book(int flightId)
    {
        try
        {
            var user = User; 
            return _flightService.BookFlight(flightId, user.UserId)
                ? ReturnValues.Succeed
                : ReturnValues.Failure;
        }
        catch (InvalidOperationException)
        {
            return ReturnValues.NotAuthenticated;
        }
    }

    private ReturnValues ModifyFlightClass(int flightId, int flightClassId)
    {
        try
        {
            var user = User; 
            return _flightService.ModifyFlight(flightId, flightClassId, user.UserId)
                ? ReturnValues.Succeed
                : ReturnValues.Failure;
        }
        catch (InvalidOperationException)
        {
            return ReturnValues.NotAuthenticated;
        }
    }

    private ReturnValues Cancel(int flightId)
    {
        try
        {
            return _flightService.CancelFlight(flightId)
                ? ReturnValues.Succeed
                : ReturnValues.Failure;
        }
        catch (InvalidOperationException)
        {
            return ReturnValues.NotAuthenticated;
        }
    }

    private List<Flight> SearchFlights(FlightSearchCriteria searchCriteria)
    {
       return _flightService.SearchFlights(searchCriteria);
    }

    private ReturnValues ShowMyFlights()
    {
        try
        {
            var user = User; 
            return  _flightService.ShowMyFlights(user.UserId) ? ReturnValues.Succeed :ReturnValues.Failure ;
        }
        catch (InvalidOperationException)
        {
            return ReturnValues.NotAuthenticated;
        }
    }

    private List<Flight>? FilterBookings(FlightFilterCriteria filterCriteria)
    {
        try
        {
            var user = User;
            if (user.Role == UserRole.Passenger)
            {
                Console.WriteLine("You must be a Manager to filter the bookings.");
                return null;
            }

           return _flightService.FilterFlights(filterCriteria);
        }
        catch (InvalidOperationException)
        {
            Console.WriteLine("You need to sign in before filtering bookings.");
            return null;
        }
    }

    private List<string>? ImportFlights(string flightsCsv)
    {
        try
        {
            var user = User;
            if (user.Role == UserRole.Passenger)
            {
                Console.WriteLine("You must be a Manager to filter the booking.");
                return null;
            }
        
            return _flightService.ImportFlightsFromCsvAsync(flightsCsv).GetAwaiter().GetResult();
        }
        catch (InvalidOperationException)
        {
            Console.WriteLine("You need to sign in before filtering bookings.");
            return null;
        }
    }
    public void SignInMenu()
    {
        _uiService.ShowSignInMenu();
    }

    public void PrintMenu()
    {
        try
        {
            var user = User;
            bool isManager = user.Role == UserRole.Manager;

            _uiService.ShowMainMenu(user.Role);

            int choice = _uiService.GetMenuChoice();
            HandleMenuChoice(choice, isManager);
        }
        catch (InvalidOperationException)
        {
            _uiService.DisplayMessage("You need to sign in before accessing the menu.");
        }
    }

    private void HandleMenuChoice(int choice, bool isManager)
    {
        switch ((MainOptions)choice)
        {
            case MainOptions.Search:
                SearchFlights(GetFlightSearchCriteria());
                break;
            case MainOptions.ShowMyFlights:
                if (ShowMyFlights() == ReturnValues.Failure)
                    Console.WriteLine("You have no booked flights.");
                break;
            case MainOptions.Book:
                var flightId = GetFlightId();
                if (flightId != -1)
                {
                    Console.WriteLine(Book(flightId) == ReturnValues.Succeed ? "Booking successful." : "Booking failed.");
                }
                else
                {
                    Console.WriteLine("Invalid Flight ID.");
                }

                break;
            case MainOptions.Modify:

                var modFlightId = GetFlightId();
                if (modFlightId == -1)
                {
                    break;
                }
                Console.Write("Flight Class (0 for Economy, 1 for Business, etc.): ");
                bool isValidFlightClass = int.TryParse(Console.ReadLine(), out var flightClassId);
                if (isValidFlightClass)
                {
                    bool isModificationSuccessful = ModifyFlightClass(modFlightId, flightClassId) == 0;
                    Console.WriteLine(isModificationSuccessful ? "Modification successful." : "Modification failed.");
                }

                break;
            case MainOptions.Cancel:
                var cancelFlightId = GetFlightId();
                if (cancelFlightId != -1)
                {
                    Console.WriteLine(Cancel(cancelFlightId) == ReturnValues.Succeed
                        ? "Cancellation successful."
                        : "Cancellation failed.");
                }

                break;
            case MainOptions.Filter when isManager:
                FilterBookings(GetFlightFilterCriteria());
                break;
            case MainOptions.ImportFlights when isManager:
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


    private int GetFlightId()
    {
        Console.Write("Enter The Flight ID : ");
        if (int.TryParse(Console.ReadLine(), out var flightId))
        {
            return flightId;
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid Flight ID.");
            return -1;
        }
    }

    static FlightFilterCriteria GetFlightFilterCriteria()
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

        return new FlightFilterCriteria
        {
            FlightId = flightId,
            Price = price,
            DepartureCountry = departureCountry,
            DestinationCountry = destinationCountry,
            DepartureDate = departureDate,
            DepartureAirport = departureAirport,
            ArrivalAirport = arrivalAirport,
            Passenger = passenger,
            FlightClass = flightClass
        };
    }

    static FlightSearchCriteria GetFlightSearchCriteria()
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
        FlightClass? flightClass = (FlightClass)TryParseInt(Console.ReadLine());

        Console.Write("Max Price: ");
        decimal? maxPrice = TryParseDecimal(Console.ReadLine());

        return new FlightSearchCriteria
        {
            DepartureCountry = departureCountry,
            DestinationCountry = destinationCountry,
            DepartureDate = departureDate,
            DepartureAirport = departureAirport,
            ArrivalAirport = arrivalAirport,
            FlightClass = flightClass,
            MaxPrice = maxPrice
        };
    }

    static int? TryParseInt(string? input) => int.TryParse(input, out var value) ? value : null;

    static decimal? TryParseDecimal(string? input) =>
        decimal.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out var value) ? value : null;

    static DateTime? TryParseDate(string? input) => DateTime.TryParseExact(input, "yyyy-MM-dd",
        CultureInfo.InvariantCulture, DateTimeStyles.None, out var date)
        ? date
        : null;
}