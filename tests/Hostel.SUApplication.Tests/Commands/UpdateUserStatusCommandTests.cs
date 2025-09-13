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
    /// Тесты на обработчик команды <see cref="UpdateUserStatusHandler"/>
    /// </summary>
    public class UpdateUserStatusCommandTests
    {
        private readonly Mock<IUserRepository> _mockRepo;
        private readonly Mock<IUnitOfWork> _mockUow;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<UpdateUserStatusHandler>> _mockLogger;

        private readonly UpdateUserStatusHandler _handler;

        public UpdateUserStatusCommandTests()
        {
            _mockRepo = new Mock<IUserRepository>();
            _mockUow = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<UpdateUserStatusHandler>>();

            _handler = new UpdateUserStatusHandler(_mockRepo.Object, _mockUow.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [Fact(DisplayName = "Удачное изменение статуса пользователя")]
        public async Task Handle_ShouldReturnSuccess_WhenUserUpdateEmailSuccessfully()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var request = new UpdateUserStatusCommand(new UserUpdateStatusRequest
            {
                Id = userId,
                StatusCode = "INACTIVE"
            });

            _mockRepo.Setup(r => r.GetByIdAsync(userId, default)).ReturnsAsync(User.Create(
                new EmailVo("test@example.com"),
                new FullNameVo("John", "Doe", "Smith"),
                "hashedPassword",
                UserTypes.Standart,  // мокаем первый тип
                UserStatuses.Active // мокаем первый статус
            ));

            _mockMapper.Setup(m => m.Map<UserResponse>(It.IsAny<User>())).Returns(new UserResponse
            {
                Id = userId,
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
            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            _mockUow.Verify(u => u.BeginTransactionAsync(CancellationToken.None), Times.Once);
            _mockUow.Verify(u => u.CommitAsync(CancellationToken.None), Times.Once);
            _mockRepo.Verify(r => r.Update(It.IsAny<User>()), Times.Once);
        }

        [Fact(DisplayName = "Неудачная попытка обновления статуса, если естатус не найден")]
        public async Task Handle_ShouldReturnException_WhenUserStatusNotExistsExists()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var request = new UpdateUserStatusCommand(new UserUpdateStatusRequest
            {
                Id = userId,
                StatusCode = "INACTIVES"
            });

            _mockRepo.Setup(r => r.GetByIdAsync(userId, default)).ReturnsAsync(User.Create(
                new EmailVo("test@example.com"),
                new FullNameVo("John", "Doe", "Smith"),
                "hashedPassword",
                UserTypes.Standart,  // мокаем первый тип
                UserStatuses.Active // мокаем первый статус
            ));

            _mockMapper.Setup(m => m.Map<UserResponse>(It.IsAny<User>())).Returns(new UserResponse
            {
                Id = userId,
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
            Assert.False(result.IsSuccess);
            Assert.Equal(DomainExceptionCodes.DomainRequiredFieldException, result.Error.Code);
            _mockUow.Verify(u => u.BeginTransactionAsync(CancellationToken.None), Times.Once);
            _mockUow.Verify(u => u.RollbackAsync(CancellationToken.None), Times.Once);
        }
    }
}
