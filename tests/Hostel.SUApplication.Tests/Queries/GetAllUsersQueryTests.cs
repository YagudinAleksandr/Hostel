using AutoMapper;
using Hostel.Shared.Application.Common;
using Hostel.Shared.Kernel;
using Hostel.SU.Application;
using Hostel.SU.Domain;
using Hostel.Users.Contracts.Response;
using Microsoft.Extensions.Logging;
using Moq;
using Hostel.Domain.Primitives;
using Xunit;

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

        [Fact(DisplayName = "Возвращает всех пользователей без фильтров")]
        public async Task Handle_ReturnsUsers_WhenNoFilters()
        {
            // Arrange
            var users = new List<User>
            {
                TestUser("test1@mail.com", "Иван", "Иванов"),
                TestUser("test2@mail.com", "Петр", "Петров")
            }.AsQueryable();
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

        [Fact(DisplayName = "Фильтрация по поисковой строке (Email и ФИО)")]
        public async Task Handle_FiltersBySearch()
        {
            // Arrange
            var users = new List<User>
            {
                TestUser("search@mail.com", "Иван", "Иванов"),
                TestUser("other@mail.com", "Петр", "Петров")
            }.AsQueryable();

            _userRepositoryMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(users);
            _mapperMock.Setup(m => m.Map<IReadOnlyList<UserResponse>>(It.IsAny<List<User>>()))
                .Returns(new List<UserResponse> { new UserResponse() });

            // Поиск по email
            var filterEmail = new QueryFilter { Search = "search" };
            var queryEmail = new GetAllUsersQuery(filterEmail);

            var resultEmail = await _handler.Handle(queryEmail, CancellationToken.None);

            Assert.True(resultEmail.IsSuccess);
            Assert.Single(resultEmail.Value);

            // Поиск по ФИО (фамилия)
            var filterName = new QueryFilter { Search = "Петров" };
            var queryName = new GetAllUsersQuery(filterName);

            _mapperMock.Setup(m => m.Map<IReadOnlyList<UserResponse>>(It.IsAny<List<User>>()))
                .Returns(new List<UserResponse> { new UserResponse() });

            var resultName = await _handler.Handle(queryName, CancellationToken.None);

            Assert.True(resultName.IsSuccess);
            Assert.Single(resultName.Value);
        }

        [Fact(DisplayName = "Фильтрация по статусу пользователя")]
        public async Task Handle_FiltersByStatus()
        {
            // Arrange
            var users = new List<User>
            {
                TestUser("a@mail.com", "Иван", "Иванов", status: "Active"),
                TestUser("b@mail.com", "Петр", "Петров", status: "Blocked")
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

        [Fact(DisplayName = "Фильтрация по типу пользователя")]
        public async Task Handle_FiltersByType()
        {
            // Arrange
            var users = new List<User>
            {
                TestUser("admin@mail.com", "Иван", "Иванов", type: "Admin"),
                TestUser("manager@mail.com", "Петр", "Петров", type: "Manager")
            }.AsQueryable();

            _userRepositoryMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(users);
            _mapperMock.Setup(m => m.Map<IReadOnlyList<UserResponse>>(It.IsAny<List<User>>()))
                .Returns(new List<UserResponse> { new UserResponse() });

            var filter = new QueryFilter
            {
                Filters = new Dictionary<string, string> { { "Type", "Manager" } }
            };
            var query = new GetAllUsersQuery(filter);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Single(result.Value);
        }

        [Fact(DisplayName = "Пагинация пользователей")]
        public async Task Handle_Pagination()
        {
            // Arrange
            var users = new List<User>();
            for (int i = 1; i <= 30; i++)
            {
                users.Add(TestUser($"user{i}@mail.com", $"Имя{i}", $"Фамилия{i}"));
            }
            var usersQueryable = users.AsQueryable();

            _userRepositoryMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(usersQueryable);
            _mapperMock.Setup(m => m.Map<IReadOnlyList<UserResponse>>(It.IsAny<List<User>>()))
                .Returns(new List<UserResponse>(Enumerable.Repeat(new UserResponse(), 10)));

            var filter = new QueryFilter
            {
                Page = 2,
                PageSize = 10
            };
            var query = new GetAllUsersQuery(filter);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(10, result.Value.Count);
        }

        [Fact(DisplayName = "Сортировка пользователей по дате создания по убыванию")]
        public async Task Handle_Sorting()
        {
            // Arrange
            var user1 = TestUser("a@mail.com", "Иван", "Иванов");
            typeof(User).GetProperty("CreatedAt")!.SetValue(user1, new DateTime(2023, 1, 1));
            var user2 = TestUser("b@mail.com", "Петр", "Петров");
            typeof(User).GetProperty("CreatedAt")!.SetValue(user2, new DateTime(2024, 1, 1));

            var users = new List<User> { user1, user2 }.AsQueryable();

            _userRepositoryMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(users);
            _mapperMock.Setup(m => m.Map<IReadOnlyList<UserResponse>>(It.IsAny<List<User>>()))
                .Returns(new List<UserResponse> { new UserResponse(), new UserResponse() });

            var filter = new QueryFilter
            {
                SortBy = "CreatedAt",
                SortDirection = "desc"
            };
            var query = new GetAllUsersQuery(filter);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Value.Count);
        }

        [Fact(DisplayName = "Возвращает ошибку при DomainException")]
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

        /// <summary>
        /// Создание тестового пользователя
        /// </summary>
        /// <param name="email">Адрес электронной почты</param>
        /// <param name="firstName">Имя</param>
        /// <param name="lastName">Фамилия</param>
        /// <param name="status">Статус</param>
        /// <param name="type">Тип</param>
        /// <returns>Пользователь</returns>
        private User TestUser(
            string email,
            string firstName = "Иван",
            string lastName = "Иванов",
            string status = "Active",
            string type = "Admin")
        {
            var emailVo = new EmailVo(email);
            var nameVo = new FullNameVo(firstName, lastName, null);

            UserStatus statusObj = UserStatuses.All.FirstOrDefault(x=>x.Code == status.ToUpper());

            UserType typeObj = UserTypes.All.FirstOrDefault(x=>x.Code==type.ToUpper());

            var user = (User)Activator.CreateInstance(typeof(User), true)!;
            typeof(User).GetProperty("Email")!.SetValue(user, emailVo);
            typeof(User).GetProperty("Name")!.SetValue(user, nameVo);
            typeof(User).GetProperty("Status")!.SetValue(user, statusObj);
            typeof(User).GetProperty("Type")!.SetValue(user, typeObj);
            typeof(User).GetProperty("CreatedAt")!.SetValue(user, DateTime.UtcNow);

            return user;
        }
    }
}
