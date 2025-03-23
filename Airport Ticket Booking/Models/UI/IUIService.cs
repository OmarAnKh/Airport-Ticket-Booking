using Airport_Ticket_Booking.Models.user;

namespace Airport_Ticket_Booking.Models.UI
{
    public interface IUiService
    {
        void ShowSignInMenu();
        void ShowMainMenu(UserRole userRole);
        int GetMenuChoice();
        void DisplayMessage(string message);
    }

}