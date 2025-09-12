using AutoMapper;
using Hostel.Domain.Primitives;
using Hostel.Shared.Kernel;
using Hostel.SU.Application;
using Hostel.SU.Domain;
using Hostel.Users.Contracts.Request;
using Hostel.Users.Contracts.Response;
using Microsoft.Extensions.Logging;
using Moq;

namespace Hostel.SUApplication.Tests.Commands
{
    /// <summary>
    /// Тесты на обработчик <see cref="CreateUserHandler"/>
    /// </summary>
    public class CreateUserCommandTests
    {
        private readonly Mock<IUserRepository> _mockRepo;
        private readonly Mock<IUnitOfWork> _mockUow;
        private readonly Mock<IPasswordService> _mockPasswordService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<CreateUserHandler>> _mockLogger;

        private readonly CreateUserHandler _handler;

        public CreateUserCommandTests()
        {
            _mockRepo = new Mock<IUserRepository>();
            _mockUow = new Mock<IUnitOfWork>();
            _mockPasswordService = new Mock<IPasswordService>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<CreateUserHandler>>();

            _handler = new CreateUserHandler(
                _mockRepo.Object,
                _mockUow.Object,
                _mockPasswordService.Object,
                _mockMapper.Object,
                _mockLogger.Object);
        }

        [Fact(DisplayName = "Удачное создание пользователя")]
        public async Task Handle_ShouldReturnSuccess_WhenUserCreatedSuccessfully()
        {
            // Arrange
            var request = new CreateUserCommand(new UserCreateRequest
            {
                Email = "test@example.com",
                Firstname = "John",
                Lastname = "Doe",
                Patronymic = "Smith",
                Password = "password123",
                Type = "STANDART",   // должен совпадать с кодом в UserTypes.All
                Status = "INACTIVE"   // должен совпадать с кодом в UserStatuses.All
            });

            var user = User.Create(
                new EmailVo("test@example.com"),
                new FullNameVo("John", "Doe", "Smith"),
                "hashedPassword",
                UserTypes.Standart,  // мокаем первый тип
                UserStatuses.Inactive // мокаем первый статус
            );

            _mockRepo.Setup(r => r.IsEmailExists("test@example.com", Guid.Empty)).ReturnsAsync(false);
            _mockPasswordService.Setup(p => p.GetHashPassword("password123")).Returns("hashedPassword");
            _mockMapper.Setup(m => m.Map<UserResponse>(It.IsAny<User>())).Returns(new UserResponse
            {
                Id = Guid.NewGuid(),
                Email = "test@example.com",
                Firstname = "John",
                Lastname = "Doe",
                Patronymic = "Smith",
                Type = "STANDART",
                Status = "INACTIVE"
            });

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            _mockUow.Verify(u => u.BeginTransactionAsync(CancellationToken.None), Times.Once);
            _mockUow.Verify(u => u.CommitAsync(CancellationToken.None), Times.Once);
            _mockRepo.Verify(r => r.Add(It.IsAny<User>()), Times.Once);
        }

        [Fact(DisplayName = "Должно вернуть неудачу, если адрес электронной почты существует")]
        public async Task Handle_ShouldReturnFailure_WhenEmailAlreadyExists()
        {
            // Arrange
            var request = new CreateUserCommand(new UserCreateRequest
            {
                Email = "existing@example.com",
                Firstname = "John",
                Lastname = "Doe",
                Password = "password123",
                Type = "STANDART",
                Status = "INACTIVE"
            });

            _mockRepo.Setup(r => r.IsEmailExists("existing@example.com", null)).ReturnsAsync(true);
            _mockPasswordService.Setup(p => p.GetHashPassword("password123")).Returns("hashedPassword");

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(DomainExceptionCodes.DomainResourceAlreadyExistsParam, result.Error.Code);
            _mockUow.Verify(u => u.RollbackAsync(CancellationToken.None), Times.Once);
        }

        [Fact(DisplayName = "Должно вернуть неудачу, если тип не найден")]
        public async Task Handle_ShouldReturnFailure_WhenTypeNotFound()
        {
            // Arrange
            var request = new CreateUserCommand(new UserCreateRequest
            {
                Email = "test@example.com",
                Firstname = "John",
                Lastname = "Doe",
                Password = "password123",
                Type = "invalid-type", // такого типа нет
                Status = "ACTIVE"
            });

            _mockRepo.Setup(r => r.IsEmailExists("test@example.com", null)).ReturnsAsync(false);
            _mockPasswordService.Setup(p => p.GetHashPassword("password123")).Returns("hashedPassword");

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(DomainExceptionCodes.DomainRequiredFieldException, result.Error.Code);
            _mockUow.Verify(u => u.RollbackAsync(CancellationToken.None), Times.Once);
        }

        [Fact(DisplayName = "Должно вернуть неудачу, если статус не найден")]
        public async Task Handle_ShouldReturnFailure_WhenStatusNotFound()
        {
            // Arrange
            var request = new CreateUserCommand(new UserCreateRequest
            {
                Email = "test@example.com",
                Firstname = "John",
                Lastname = "Doe",
                Password = "password123",
                Type = "STANDART",
                Status = "invalid-status" // такого статуса нет
            });

            _mockRepo.Setup(r => r.IsEmailExists("test@example.com", Guid.Empty)).ReturnsAsync(false);
            _mockPasswordService.Setup(p => p.GetHashPassword("password123")).Returns("hashedPassword");

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(DomainExceptionCodes.DomainRequiredFieldException, result.Error.Code);
            _mockUow.Verify(u => u.RollbackAsync(CancellationToken.None), Times.Once);
        }

        [Fact(DisplayName = "Должен вернуть исключение репозитория")]
        public async Task Handle_ShouldReturnFailure_OnUnexpectedException()
        {
            // Arrange
            var request = new CreateUserCommand(new UserCreateRequest
            {
                Email = "test@example.com",
                Firstname = "John",
                Lastname = "Doe",
                Password = "password123",
                Type = "STANDART",
                Status = "INACTIVE"
            });

            _mockRepo.Setup(r => r.IsEmailExists("test@example.com", null)).ReturnsAsync(false);
            _mockPasswordService.Setup(p => p.GetHashPassword("password123")).Returns("hashedPassword");
            _mockRepo.Setup(r => r.Add(It.IsAny<User>())).Throws(new Exception("Unexpected error"));

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(DomainExceptionCodes.InternalServerError, result.Error.Code);
            _mockUow.Verify(u => u.RollbackAsync(CancellationToken.None), Times.Once);
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}
