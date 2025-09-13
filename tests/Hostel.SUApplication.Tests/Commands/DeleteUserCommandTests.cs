using Hostel.Domain.Primitives;
using Hostel.Shared.Kernel;
using Hostel.SU.Application;
using Hostel.SU.Domain;
using Microsoft.Extensions.Logging;
using Moq;

namespace Hostel.SUApplication.Tests.Commands
{
    /// <summary>
    /// Тесты на обработчик команды <see cref="DeleteUserHandler"/>
    /// </summary>
    public class DeleteUserCommandTests
    {
        private readonly Mock<IUserRepository> _mockRepo;
        private readonly Mock<IUnitOfWork> _mockUow;
        private readonly Mock<ILogger<DeleteUserHandler>> _mockLogger;

        private readonly DeleteUserHandler _handler;

        public DeleteUserCommandTests()
        {
            _mockRepo = new Mock<IUserRepository>();
            _mockUow = new Mock<IUnitOfWork>();
            _mockLogger = new Mock<ILogger<DeleteUserHandler>>();

            _handler = new DeleteUserHandler(_mockRepo.Object, _mockUow.Object, _mockLogger.Object);
        }

        [Fact(DisplayName = "Удачное удаление пользователя")]
        public async Task Handle_ShouldReturnSuccess_WhenUserDeleteSuccessfully()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var request = new DeleteUserCommand(userId);

            _mockRepo.Setup(r => r.GetByIdAsync(userId, default)).ReturnsAsync(User.Create(
                new EmailVo("test@example.com"),
                new FullNameVo("First name", "Last name", "Patronymic"),
                "hashedPassword",
                UserTypes.Standart,
                UserStatuses.Active
            ));

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            _mockUow.Verify(u => u.BeginTransactionAsync(CancellationToken.None), Times.Once);
            _mockUow.Verify(u => u.CommitAsync(CancellationToken.None), Times.Once);
            _mockRepo.Verify(r => r.Delete(It.IsAny<User>()), Times.Once);
        }

        [Fact(DisplayName = "Неудачная попытка удалить пользователя, если пользователь не найден в базе")]
        public async Task Handle_ShouldReturnException_WhenUserNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var request = new DeleteUserCommand(userId);

            _mockRepo.Setup(r => r.GetByIdAsync(userId, default)).ReturnsAsync(default(User));

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(DomainExceptionCodes.DomainResourceNotFoundWithParam, result.Error.Code);
            _mockUow.Verify(u => u.BeginTransactionAsync(CancellationToken.None), Times.Once);
            _mockUow.Verify(u => u.RollbackAsync(CancellationToken.None), Times.Once);
        }
    }
}
