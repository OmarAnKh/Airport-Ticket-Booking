﻿// See https://aka.ms/new-console-template for more information

using Airport_Ticket_Booking.Models.flight;
using Airport_Ticket_Booking.Models.user;

var repo = new UserRepository("../../../Data/users.txt");
repo.GetAllData();
Console.WriteLine(repo.Authontication(new User("Ahmad", "Ahmad@1234")));
var flightrepo = new FlightRepository("../../../Data/flight.txt");
flightrepo.GetAllData();
Console.Read();