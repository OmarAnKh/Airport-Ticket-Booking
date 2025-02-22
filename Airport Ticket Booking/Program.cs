// See https://aka.ms/new-console-template for more information

using Airport_Ticket_Booking.Models;

var temp = ApplicationServices.GetInstance();
if (temp == null) return ;
temp.SignIn("Ahmad", "Ahmad@1234");
temp.Book(102);
temp.ShowMyFlights();
