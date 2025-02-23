namespace Airport_Ticket_Booking.Models.user;

public record User(string Username, string Password, UserRole Role =0, int UserId = 0);