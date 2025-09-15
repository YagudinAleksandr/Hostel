using AutoMapper;
using Hostel.Shared.Application.Common;
using Hostel.Shared.Kernel;
using Hostel.SU.Application;
using Hostel.SU.Domain;
using Hostel.Users.Contracts.Response;
using Microsoft.Extensions.Logging;
using Moq;
using Hostel.Domain.Primitives;

namespace Hostel.SUApplication.Tests.Queries
{
    public class GetAllUsersQueryTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<GetAllUsersHandler>> _loggerMock;
        private readonly GetAllUsersHandler _handler;

        public GetAllUsersQueryTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<GetAllUsersHandler>>();
            _handler = new GetAllUsersHandler(_userRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsUsers_WhenNoFilters()
        {
            // Arrange
            var users = new List<User> { TestUser("test1@mail.com"), TestUser("test2@mail.com") }.AsQueryable();
            var userResponses = new List<UserResponse> { new UserResponse(), new UserResponse() };

            _userRepositoryMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(users);
            _mapperMock.Setup(m => m.Map<IReadOnlyList<UserResponse>>(It.IsAny<List<User>>())).Returns(userResponses);

            var query = new GetAllUsersQuery(new QueryFilter());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Value.Count);
        }

        [Fact]
        public async Task Handle_FiltersBySearch()
        {
            // Arrange
            var users = new List<User>
            {
                TestUser("search@mail.com", "Иванов Иван"),
                TestUser("other@mail.com", "Петров Петр")
            }.AsQueryable();

            _userRepositoryMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(users);
            _mapperMock.Setup(m => m.Map<IReadOnlyList<UserResponse>>(It.IsAny<List<User>>()))
                .Returns(new List<UserResponse> { new UserResponse() });

            var filter = new QueryFilter { Search = "search" };
            var query = new GetAllUsersQuery(filter);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Single(result.Value);
        }

        [Fact]
        public async Task Handle_FiltersByStatus()
        {
            // Arrange
            var users = new List<User>
            {
                TestUser("a@mail.com", status: "Active"),
                TestUser("b@mail.com", status: "Blocked")
            }.AsQueryable();

            _userRepositoryMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(users);
            _mapperMock.Setup(m => m.Map<IReadOnlyList<UserResponse>>(It.IsAny<List<User>>()))
                .Returns(new List<UserResponse> { new UserResponse() });

            var filter = new QueryFilter
            {
                Filters = new Dictionary<string, string> { { "Status", "Active" } }
            };
            var query = new GetAllUsersQuery(filter);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Single(result.Value);
        }

        [Fact]
        public async Task Handle_ReturnsFailure_OnDomainException()
        {
            // Arrange
            _userRepositoryMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new DomainException("ERR", new[] { "param" }));

            var query = new GetAllUsersQuery(new QueryFilter());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("ERR", result.Error.Code);
        }

        [Fact(DisplayName = "Возвращает ошибку при Exception")]
        public async Task Handle_ReturnsFailure_OnGeneralException()
        {
            // Arrange
            _userRepositoryMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("fail"));

            var query = new GetAllUsersQuery(new QueryFilter());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(Hostel.Shared.Kernel.DomainExceptionCodes.InternalServerError, result.Error.Code);
        }

        private User TestUser(string email, string name = "Иванов Иван", string status = "Active", string type = "Admin")
        {
            var emailVo = (EmailVo)Activator.CreateInstance(
                typeof(EmailVo), true)!;
            typeof(EmailVo).GetProperty("Value")!.SetValue(emailVo, email);

            var nameVo = (FullNameVo)Activator.CreateInstance(
                typeof(FullNameVo), true)!;
            typeof(FullNameVo).GetProperty("Value")!.SetValue(nameVo, name);

            var statusObj = new TestStatus(status);
            var typeObj = new TestType(type);

            var user = (User)Activator.CreateInstance(typeof(User), true)!;
            typeof(User).GetProperty("Email")!.SetValue(user, emailVo);
            typeof(User).GetProperty("Name")!.SetValue(user, nameVo);
            typeof(User).GetProperty("Status")!.SetValue(user, statusObj);
            typeof(User).GetProperty("Type")!.SetValue(user, typeObj);
            typeof(User).GetProperty("CreatedAt")!.SetValue(user, DateTime.UtcNow);

            return user;
        }

        private class TestStatus
        {
            public string DisplayName { get; }
            public TestStatus(string displayName) => DisplayName = displayName;
        }

        private class TestType
        {
            public string DisplayName { get; }
            public TestType(string displayName) => DisplayName = displayName;
        }
    }
}
