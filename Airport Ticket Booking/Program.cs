// See https://aka.ms/new-console-template for more information

using Airport_Ticket_Booking.Models.flight;
using Airport_Ticket_Booking.Models.user;

var temp = new UserServices("../../../Data/users.txt");
temp.SignUp("Omar", "Omar@1234");
