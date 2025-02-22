// See https://aka.ms/new-console-template for more information

using Airport_Ticket_Booking.Models;
using Airport_Ticket_Booking.Models.flight;
using Airport_Ticket_Booking.Models.user;

var temp = ApplicationServices.GetInstance();
temp.SignIn("Ahmad", "Ahmad@1234");
Console.WriteLine(temp.Book(101));
