using Airport_Ticket_Booking.Models.user;

namespace Airport_Ticket_Booking.Models.UI
{
    public class UiService : IUiService
    {
        public void ShowSignInMenu()
        {
            Console.WriteLine("1) Sign In");
            Console.WriteLine("2) Sign Up");
            Console.WriteLine("3) Exit");
        }

        public void ShowMainMenu(UserRole userRole)
        {
            bool isManager = userRole == UserRole.Manager;
        
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
        }

        public int GetMenuChoice()
        {
            Console.Write("Enter your choice: ");
            return int.TryParse(Console.ReadLine(), out int choice) ? choice : -1;
        }

        public void DisplayMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
    

}