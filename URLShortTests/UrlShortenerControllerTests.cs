using Microsoft.AspNetCore.Mvc;
using Moq;
using URLShort.Controllers;
using URLShort.Models;
using URLShort.Repositories.Interface;

namespace URLShortTests
{
    public class UrlShortenerControllerTests
    {
        private readonly Mock<IUrlShortenerRepository> _repoMock;
        private readonly UrlShortenerController _controller;

        public UrlShortenerControllerTests()
        {
            _repoMock = new Mock<IUrlShortenerRepository>();
            _controller = new UrlShortenerController(_repoMock.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOk_WithUrlList()
        {
            // Arrange
            var urls = new List<UrlShortener>
            {
                new UrlShortener { Id = 1, OriginalUrl = "https://google.com", ShortenedUrl = "abc" },
                new UrlShortener { Id = 2, OriginalUrl = "https://github.com", ShortenedUrl = "def" }
            };

            _repoMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(urls);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedUrls = Assert.IsType<List<UrlShortener>>(okResult.Value);
            Assert.Equal(2, returnedUrls.Count);
        }

        [Fact]
        public async Task GetById_ReturnsOk_WhenExists()
        {
            // Arrange
            var url = new UrlShortener
            {
                Id = 1,
                OriginalUrl = "https://google.com",
                ShortenedUrl = "abc"
            };

            _repoMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(url);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedUrl = Assert.IsType<UrlShortener>(okResult.Value);
            Assert.Equal("abc", returnedUrl.ShortenedUrl);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenMissing()
        {
            // Arrange
            _repoMock
                .Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((UrlShortener?)null);

            // Act
            var result = await _controller.GetById(99);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetByShortenedUrl_ReturnsOk_WhenExists()
        {
            // Arrange
            var url = new UrlShortener
            {
                Id = 1,
                OriginalUrl = "https://google.com",
                ShortenedUrl = "abc"
            };

            _repoMock
                .Setup(r => r.GetByShortenedUrlAsync("abc"))
                .ReturnsAsync(url);

            // Act
            var result = await _controller.GetByShortenedUrl("abc");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedUrl = Assert.IsType<UrlShortener>(okResult.Value);
            Assert.Equal(1, returnedUrl.Id);
        }

        [Fact]
        public async Task GetByShortenedUrl_ReturnsNotFound_WhenMissing()
        {
            // Arrange
            _repoMock
                .Setup(r => r.GetByShortenedUrlAsync("zzz"))
                .ReturnsAsync((UrlShortener?)null);

            // Act
            var result = await _controller.GetByShortenedUrl("zzz");

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtAction()
        {
            // Arrange
            var url = new UrlShortener
            {
                Id = 10,
                OriginalUrl = "https://example.com"
            };

            _repoMock
                .Setup(r => r.AddAsync(url))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(url);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(nameof(UrlShortenerController.GetById), createdResult.ActionName);
            Assert.Equal(url, createdResult.Value);
        }

        [Fact]
        public async Task DeleteById_ReturnsNoContent_WhenExists()
        {
            // Arrange
            var url = new UrlShortener { Id = 1 };

            _repoMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(url);

            _repoMock
                .Setup(r => r.DeleteByIdAsync(1))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteById(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteById_ReturnsNotFound_WhenMissing()
        {
            // Arrange
            _repoMock
                .Setup(r => r.GetByIdAsync(42))
                .ReturnsAsync((UrlShortener?)null);

            // Act
            var result = await _controller.DeleteById(42);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
