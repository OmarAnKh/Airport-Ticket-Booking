// See https://aka.ms/new-console-template for more information

using Airport_Ticket_Booking.Models;

var temp = ApplicationServices.GetInstance();
if (temp == null) return;

var errors = temp.ImportFlights("../../../Data/DataToImport.txt");
foreach (var error in errors)
{
    Console.WriteLine(error);
}