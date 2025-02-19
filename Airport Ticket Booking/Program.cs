// See https://aka.ms/new-console-template for more information

using Airport_Ticket_Booking.Models.flight;
using Airport_Ticket_Booking.Models.user;

var flights = Flights.GetInstance("../../../Data/flight.txt");
flights.SearchFlights("Germany", "Italy", new DateTime(2025, 3, 10, 14, 45, 0), "Munich", "Fiumicino", "Business", 350);
flights.SearchFlights("France", "Germany", new DateTime(2025, 4, 20, 14, 0, 0), "Frankfurt", "67890",
    "Economy", 102m);
Console.Read();