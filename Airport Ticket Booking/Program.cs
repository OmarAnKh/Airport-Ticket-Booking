// See https://aka.ms/new-console-template for more information

using Airport_Ticket_Booking.Models.flight;
using Airport_Ticket_Booking.Models.user;

var flights = Flights.GetInstance("../../../Data/flight.txt");
flights.CancelFlight(101);
