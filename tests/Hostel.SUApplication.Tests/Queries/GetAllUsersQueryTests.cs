using AutoMapper;
using Hostel.Domain.Primitives;
using Hostel.Shared.Application.Common;
using Hostel.Shared.Kernel;
using Hostel.SU.Application;
using Hostel.SU.Domain;
using Hostel.Users.Contracts.Response;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hostel.SUApplication.Tests.Queries
{
    public class GetAllUsersQueryTests
    {
        private readonly Mock<IUserRepository> _repositoryMock = new Mock<IUserRepository>();
        private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();
        private readonly Mock<ILogger<GetAllUsersHandler>> _loggerMock = new Mock<ILogger<GetAllUsersHandler>>();
        private readonly GetAllUsersHandler _handler;

        public GetAllUsersQueryTests()
        {
            _handler = new GetAllUsersHandler(_repositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact(DisplayName = "Должен вернуть пустой результат, когда репозиторий возвращает null")]
        public async Task Handle_WhenRepositoryReturnsNull_ShouldReturnEmptyPagedResult()
        {
            // Arrange
            var filter = new UnifiedFilter { PageNumber = 1, PageSize = 10 };
            var query = new GetAllUsersQuery(filter);

            _repositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync((IQueryable<User>)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value.Items);
            Assert.Equal(0, result.Value.TotalCount);
            Assert.Equal(1, result.Value.PageNumber);
            Assert.Equal(10, result.Value.PageSize);
        }

        [Fact(DisplayName = "Должен вернуть пустой результат, когда репозиторий возвращает пустой список")]
        public async Task Handle_WhenRepositoryReturnsEmptyList_ShouldReturnEmptyPagedResult()
        {
            // Arrange
            var filter = new UnifiedFilter { PageNumber = 1, PageSize = 10 };
            var query = new GetAllUsersQuery(filter);
            var emptyUsers = new List<User>().AsQueryable();

            _repositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(emptyUsers);

            _mapperMock.Setup(x => x.Map<List<UserResponse>>(It.IsAny<List<User>>()))
                .Returns(new List<UserResponse>());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value.Items);
            Assert.Equal(0, result.Value.TotalCount);
            Assert.Equal(1, result.Value.PageNumber);
            Assert.Equal(10, result.Value.PageSize);
        }

        [Fact(DisplayName = "Должен успешно вернуть пользователей с пагинацией")]
        public async Task Handle_WithUsers_ShouldReturnPagedResult()
        {
            // Arrange
            var filter = new UnifiedFilter { PageNumber = 1, PageSize = 2 };
            var query = new GetAllUsersQuery(filter);

            var users = new List<User>
            {
                CreateTestUser("User1", "user1@test.com"),
                CreateTestUser("User2", "user2@test.com"),
                CreateTestUser("User3", "user3@test.com")
            }.AsQueryable();

            var userResponses = new List<UserResponse>
            {
                CreateTestUserResponse(users.ElementAt(0).Id, "User1"),
                CreateTestUserResponse(users.ElementAt(1).Id, "User2")
            };

            _repositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(users);

            _mapperMock.Setup(x => x.Map<List<UserResponse>>(It.IsAny<List<User>>()))
                .Returns(userResponses);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(2, result.Value.Items.Count());
            Assert.Equal(3, result.Value.TotalCount);
            Assert.Equal(1, result.Value.PageNumber);
            Assert.Equal(2, result.Value.PageSize);
        }

        [Fact(DisplayName = "Должен применить сортировку по умолчанию, когда не указаны опции сортировки")]
        public async Task Handle_WhenNoSortOptions_ShouldApplyDefaultSort()
        {
            // Arrange
            var filter = new UnifiedFilter { PageNumber = 1, PageSize = 10 };
            var query = new GetAllUsersQuery(filter);

            var users = new List<User>
            {
                CreateTestUser("UserC", "userc@test.com"),
                CreateTestUser("UserA", "usera@test.com"),
                CreateTestUser("UserB", "userb@test.com")
            }.AsQueryable();

            var sortedUsers = users.OrderBy(u => u.Name).ToList();
            var userResponses = sortedUsers.Select(u => CreateTestUserResponse(u.Id, u.Name.ToString())).ToList();

            _repositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(users);

            _mapperMock.Setup(x => x.Map<List<UserResponse>>(It.IsAny<List<User>>()))
                .Returns(userResponses);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("UserA", result.Value.Items.First().Firstname);
            Assert.Equal("UserC", result.Value.Items.Last().Firstname);
        }

        [Fact(DisplayName = "Должен корректно обработать исключение и вернуть ошибку")]
        public async Task Handle_WhenExceptionThrown_ShouldReturnFailureResult()
        {
            // Arrange
            var filter = new UnifiedFilter { PageNumber = 1, PageSize = 10 };
            var query = new GetAllUsersQuery(filter);

            var exceptionMessage = "Database connection failed";
            _repositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.NotNull(result.Error);
            Assert.Equal(DomainExceptionCodes.InternalServerError, result.Error.Code);

            // Verify logging
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(exceptionMessage)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact(DisplayName = "Должен применить пагинацию и пропустить указанное количество элементов")]
        public async Task Handle_WithPagination_ShouldSkipAndTakeCorrectly()
        {
            // Arrange
            var filter = new UnifiedFilter { PageNumber = 2, PageSize = 2 };
            var query = new GetAllUsersQuery(filter);

            var users = new List<User>
            {
                CreateTestUser("User1", "user1@test.com"),
                CreateTestUser("User2", "user2@test.com"),
                CreateTestUser("User3", "user3@test.com"),
                CreateTestUser("User4", "user4@test.com")
            }.AsQueryable();

            var pagedUsers = users.Skip(2).Take(2).ToList();
            var userResponses = pagedUsers.Select(u => CreateTestUserResponse(u.Id, u.Name.ToString())).ToList();

            _repositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(users);

            _mapperMock.Setup(x => x.Map<List<UserResponse>>(It.IsAny<List<User>>()))
                .Returns(userResponses);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Value.Items.Count());
            Assert.Equal(4, result.Value.TotalCount);
            Assert.Equal("User3", result.Value.Items.First().Firstname);
            Assert.Equal("User4", result.Value.Items.Last().Firstname);
        }

        [Fact(DisplayName = "Должен вернуть успешный результат с правильными метаданными пагинации")]
        public async Task Handle_ShouldReturnCorrectPaginationMetadata()
        {
            // Arrange
            var filter = new UnifiedFilter { PageNumber = 2, PageSize = 3 };
            var query = new GetAllUsersQuery(filter);

            var users = new List<User>
            {
                CreateTestUser("User1", "user1@test.com"),
                CreateTestUser("User2", "user2@test.com"),
                CreateTestUser("User3", "user3@test.com"),
                CreateTestUser("User4", "user4@test.com"),
                CreateTestUser("User5", "user5@test.com")
            }.AsQueryable();

            var userResponses = users.Skip(3).Take(3).Select(u => CreateTestUserResponse(u.Id, u.Name.ToString())).ToList();

            _repositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(users);

            _mapperMock.Setup(x => x.Map<List<UserResponse>>(It.IsAny<List<User>>()))
                .Returns(userResponses);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(5, result.Value.TotalCount);
            Assert.Equal(2, result.Value.PageNumber);
            Assert.Equal(3, result.Value.PageSize);
            Assert.Equal(2, result.Value.Items.Count()); // 2 элемента на второй странице при размере страницы 3
        }

        [Fact(DisplayName = "Должен применить динамическую сортировку, когда указаны опции сортировки")]
        public async Task Handle_WithSortOptions_ShouldApplyDynamicSorting()
        {
            // Arrange
            var filter = new UnifiedFilter
            {
                PageNumber = 1,
                PageSize = 10,
                SortOptions = new List<SortOption>
                {
                    new SortOption { Field = "Email", Direction = "desc" }
                }
            };
            var query = new GetAllUsersQuery(filter);

            var users = new List<User>
            {
                CreateTestUser("User1", "userA@test.com"),
                CreateTestUser("User2", "userC@test.com"),
                CreateTestUser("User3", "userB@test.com")
            }.AsQueryable();

            var userResponses = users.Select(u => CreateTestUserResponse(u.Id, u.Name.ToString())).ToList();

            _repositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(users);

            _mapperMock.Setup(x => x.Map<List<UserResponse>>(It.IsAny<List<User>>()))
                .Returns(userResponses);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(3, result.Value.Items.Count());
        }

        [Fact(DisplayName = "Должен обработать выборку полей, когда указаны SelectFields")]
        public async Task Handle_WithSelectFields_ShouldApplyProjection()
        {
            // Arrange
            var filter = new UnifiedFilter
            {
                PageNumber = 1,
                PageSize = 10,
                SelectFields = new List<string> { "Id", "Firstname", "Email" }
            };
            var query = new GetAllUsersQuery(filter);

            var users = new List<User>
            {
                CreateTestUser("User1", "user1@test.com"),
                CreateTestUser("User2", "user2@test.com")
            }.AsQueryable();

            var userResponses = users.Select(u => CreateTestUserResponse(u.Id, u.Name.ToString())).ToList();

            _repositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(users);

            _mapperMock.Setup(x => x.Map<List<UserResponse>>(It.IsAny<List<User>>()))
                .Returns(userResponses);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(2, result.Value.Items.Count());
        }

        private User CreateTestUser(string name, string email)
        {
            var fullNameVo = new FullNameVo(name, name, null);
            var emailVo = new EmailVo(email);
            var userType = UserTypes.Standart;
            var userStatus = UserStatuses.Active;

            return User.Create(emailVo, fullNameVo, "passwordHash", userType, userStatus);
        }

        private UserResponse CreateTestUserResponse(Guid id, string firstname)
        {
            return new UserResponse
            {
                Id = id,
                Firstname = firstname,
                Lastname = "TestLastname",
                Email = "test@email.com",
                Type = "TestType",
                Status = "Active",
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
