using Airport_Ticket_Booking.Models.user;
using Airport_Ticket_Booking.Models.user.Repositories.Interfaces;
using Airport_Ticket_Booking.Models.user.Services;
using AutoFixture;
using Moq;

namespace Airport_Ticket_Booking.Test
{
    public class UserServiceShould
    {
        private readonly  Mock<IUserRepository> _userRepository = new();
        
        [Theory]
        [InlineData("testuser1", "password123", true)]  // User does not exist → should return true
        [InlineData("existingUser", "securePass456", false)] // User exists → should return false
        public void RegisterUserShouldReturnExpectedResultBasedOnUserExistence(
            string username, string password, bool expectedResult)
        {
            // Arrange
            var existingUser = expectedResult ? null : new User(username, "hashedPassword", UserRole.Passenger, 1);
        
            _userRepository.Setup(repo => repo.Authentication(username)).Returns(existingUser);
            _userRepository.Setup(repo => repo.GetAllData()).Returns(new List<User>());
            _userRepository.Setup(repo => repo.Create(It.IsAny<User>())).Returns(true);
        

            var userService = new UserService(_userRepository.Object);
        
            // Act
            bool result = userService.RegisterUser(username, password);
        
            // Assert
            Assert.Equal(expectedResult, result);
            _userRepository.Verify(repo => repo.Authentication(username), Times.Once);
        
            if (expectedResult)
            {
                _userRepository.Verify(repo => repo.Create(It.IsAny<User>()), Times.Once);
            }
        }

        [Theory]
        [InlineData("testuser1", "$2a$11$L/NVPEvlNI1q48hWFBhbJuLAMCzhMBv5ZkMh6JOoQEa2sQHMDcIa2", false, null)] // Correct username & password, but no user with this credential should return null
        [InlineData("testuser1", "NotRightPassword", false, null)] // Incorrect password, should return null
        [InlineData("existingUser", "$2a$11$L/NVPEvlNI1q48hWFBhbJuLAMCzhMBv5ZkMh6JOoQEa2sQHMDcIa2", true, "existingUser")] // Correct username & password, should return user
        public void AuthenticateUser_Should_ReturnExpectedResult(
            string username, string password, bool userExists, string? expectedUsername)
        {
            // Arrange
            User? existingUser = userExists ? new User(username, password, UserRole.Passenger, 1) : null;
            _userRepository.Setup(repo => repo.Authentication(username)).Returns(existingUser);

            var userService = new UserService(_userRepository.Object);

            // Act
            User? result = userService.AuthenticateUser(username, "Abood@1234");

            // Assert
            if (expectedUsername == null)
            {
                Assert.Null(result);
            }
            else
            {
                Assert.NotNull(result);
                Assert.Equal(expectedUsername, result.Username);
            }

            _userRepository.Verify(repo => repo.Authentication(username), Times.Once);
        }
    }
}