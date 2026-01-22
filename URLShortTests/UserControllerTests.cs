using Microsoft.AspNetCore.Mvc;
using Moq;
using URLShort.Controllers;
using URLShort.Models;
using URLShort.Repositories.Interface;

namespace URLShortTests
{
    public class UserControllerTests
    {
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _controller = new UserController(_userRepoMock.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOk_WithUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, Username = "admin" },
                new User { Id = 2, Username = "user" }
            };

            _userRepoMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(users);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedUsers = Assert.IsType<List<User>>(okResult.Value);
            Assert.Equal(2, returnedUsers.Count);
        }

        [Fact]
        public async Task GetById_ReturnsOk_WhenUserExists()
        {
            // Arrange
            var user = new User { Id = 1, Username = "admin" };

            _userRepoMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(user);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedUser = Assert.IsType<User>(okResult.Value);
            Assert.Equal("admin", returnedUser.Username);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            _userRepoMock
                .Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _controller.GetById(99);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetByUsername_ReturnsOk_WhenUserExists()
        {
            // Arrange
            var user = new User { Id = 1, Username = "admin" };

            _userRepoMock
                .Setup(r => r.GetByUserNameAsync("admin"))
                .ReturnsAsync(user);

            // Act
            var result = await _controller.GetByUsername("admin");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedUser = Assert.IsType<User>(okResult.Value);
            Assert.Equal(1, returnedUser.Id);
        }

        [Fact]
        public async Task GetByUsername_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            _userRepoMock
                .Setup(r => r.GetByUserNameAsync("ghost"))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _controller.GetByUsername("ghost");

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtAction()
        {
            // Arrange
            var user = new User { Id = 10, Username = "newuser" };

            _userRepoMock
                .Setup(r => r.AddAsync(user))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(user);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(nameof(UserController.GetById), createdResult.ActionName);
            Assert.Equal(user, createdResult.Value);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenUserExists()
        {
            // Arrange
            var user = new User { Id = 1, Username = "admin" };

            _userRepoMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(user);

            _userRepoMock
                .Setup(r => r.DeleteByIdAsync(1))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            _userRepoMock
                .Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _controller.Delete(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
