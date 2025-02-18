// See https://aka.ms/new-console-template for more information
using Airport_Ticket_Booking.Models.user;

var repo = new UserRepository("../../../Data/users.txt");
repo.GetAllData();
repo.Create(new User("Ahmad", "Ahmad@1234"));
Console.Read();