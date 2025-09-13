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
    /// Тесты на обработчик команды <see cref="UpdateUserPasswordHandler"/>
    /// </summary>
    public class UpdateUserPasswordCommandTests
    {
        private readonly Mock<IUserRepository> _mockRepo;
        private readonly Mock<IUnitOfWork> _mockUow;
        private readonly Mock<ILogger<UpdateUserPasswordHandler>> _mockLogger;
        private readonly Mock<IPasswordService> _passwordMock;

        private readonly UpdateUserPasswordHandler _handler;

        public UpdateUserPasswordCommandTests()
        {
            _mockRepo = new Mock<IUserRepository>();
            _mockUow = new Mock<IUnitOfWork>();
            _mockLogger = new Mock<ILogger<UpdateUserPasswordHandler>>();
            _passwordMock = new Mock<IPasswordService>();

            _handler = new UpdateUserPasswordHandler(_mockRepo.Object, _mockUow.Object, _passwordMock.Object, _mockLogger.Object);
        }

        [Fact(DisplayName = "Удачное обновление пароля")]
        public async Task Handle_ShouldReturnSuccess_WhenUserUpdatePassswordSuccessfully()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var request = new UpdateUserPasswordCommand(new UserUpdatePasswordRequest()
            {
                Id = userId,
                NewPassword = "pass123",
                OldPassword = "hashedPass",
                ConfirmedNewPassword = "hashedPass"
            });

            _passwordMock.Setup(p => p.ValidateHash("hashedPass", "hashedPass")).Returns(true);
            _mockRepo.Setup(r => r.GetByIdAsync(userId, default)).ReturnsAsync(User.Create(
                new EmailVo("test@example.com"),
                new FullNameVo("First name", "Last name", "Patronymic"),
                "hashedPass",
                UserTypes.Standart,
                UserStatuses.Active
            ));
            _passwordMock.Setup(hp => hp.GetHashPassword("pass123")).Returns("pass123");

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            _mockUow.Verify(u => u.BeginTransactionAsync(CancellationToken.None), Times.Once);
            _mockUow.Verify(u => u.CommitAsync(CancellationToken.None), Times.Once);
            _mockRepo.Verify(r => r.Update(It.IsAny<User>()), Times.Once);
        }

        [Fact(DisplayName = "Должен вернуть исключение, когда пользователь не найден")]
        public async Task Handle_ShouldReturnFailure_WhenOldPasswordNotCorrect()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var request = new UpdateUserPasswordCommand(new UserUpdatePasswordRequest()
            {
                Id = userId,
                NewPassword = "pass123",
                OldPassword = "hashedPass",
                ConfirmedNewPassword = "hashedPass"
            });

            _passwordMock.Setup(p => p.ValidateHash("hashedPass", "hashedPass")).Returns(false);
            _mockRepo.Setup(r => r.GetByIdAsync(userId, default)).ReturnsAsync(User.Create(
                new EmailVo("test@example.com"),
                new FullNameVo("First name", "Last name", "Patronymic"),
                "hashedPass",
                UserTypes.Standart,
                UserStatuses.Active
            ));
            _passwordMock.Setup(hp => hp.GetHashPassword("pass123")).Returns("pass123");

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(DomainExceptionCodes.DomainValidateFieldException, result.Error.Code);
            Assert.Equal(ServicesUsersFieldCodes.ServicesUsersFieldOldPassword, result.Error.Parameters[0]);
            _mockUow.Verify(u => u.RollbackAsync(CancellationToken.None), Times.Once);
        }
    }
}
