namespace Airport_Ticket_Booking.Models.user;

public record User(string Username, string Password, string Role = "Passenger", int UserId = 0);