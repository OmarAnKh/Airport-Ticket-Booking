// See https://aka.ms/new-console-template for more information

using Airport_Ticket_Booking.Models;
using Airport_Ticket_Booking.Models.flight;
using Airport_Ticket_Booking.Models.flight.Repositories;
using Airport_Ticket_Booking.Models.user.Repositories;
using Airport_Ticket_Booking.Models.user.Services;

namespace Airport_Ticket_Booking;

static class Program
{
    public static void Main()
    {
        var userRepository = UserRepository.GetInstance("../../../Data/users.txt");
        var userServices = new UserServices(userRepository);
        var flightRepository = FlightRepository.GetInstance("../../../Data/flight.txt");
        var flightSearchServices = new FlightSearchService();
        var flightFilterService = new FlightFilterService();
        var bookingManager = new BookingManager();
        var flightServices =
            new FlightServices(flightSearchServices, flightRepository, bookingManager, flightFilterService);

        var appServices = ApplicationServices.GetInstance(flightServices, userServices);

        while (true)
        {
            Console.WriteLine("Welcome to the Airport Ticket Booking System!");
            appServices.SignInMenu();
            Console.Write("Choose an option: ");

            if (!int.TryParse(Console.ReadLine(), out int option))
            {
                continue;
            }

            string? username;
            string? password;
            switch (option)
            {
                case (int)SignInMenu.SignIn:
                    Console.Write("Enter username: ");
                    username = Console.ReadLine();
                    Console.Write("Enter password: ");
                    password = Console.ReadLine();

                    if (appServices.SignIn(username!, password!) == (int)ReturnValues.Succeed)
                    {
                        RunUserMenu(appServices);
                    }

                    break;
                case (int)SignInMenu.SignUp:
                    Console.Write("Enter username: ");
                    username = Console.ReadLine();
                    Console.Write("Enter password: ");
                    password = Console.ReadLine();
                    if (appServices.SignUp(username!, password!))
                    {
                        RunUserMenu(appServices);
                    }
                    else
                    {
                        Console.WriteLine("Username is used.");
                    }

                    break;
                case (int)SignInMenu.Exit:
                    Console.WriteLine("Exiting... Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    static void RunUserMenu(ApplicationServices appServices)
    {
        int menuOption = 1;
        while (menuOption != 0)
        {
            Console.WriteLine("\nYour Menu Options:");
            appServices.PrintMenu();
            Console.WriteLine("0)To exit\n1)To continue");
            int.TryParse(Console.ReadLine(), out menuOption);
        }
    }
}