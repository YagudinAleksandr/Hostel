using Hostel.Domain.Primitives;
using Hostel.Shared.Kernel;
using Hostel.SU.Application;
using Hostel.SU.Domain;
using Hostel.Users.Contracts.Request;
using Microsoft.Extensions.Logging;
using Moq;

namespace Hostel.SUApplication.Tests.Commands
{
    /// <summary>
    /// Тесты на команду <see cref="UpdateResetTokenPasswordHandler"/>
    /// </summary>
    public class UpdateResetTokenPasswordCommandTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository = new Mock<IUserRepository>();
        private readonly Mock<ILogger<UpdateResetTokenPasswordHandler>> _mockLogger = new Mock<ILogger<UpdateResetTokenPasswordHandler>>();

        private readonly Mock<IResetPasswordTokenRepository> _mockTokenRepository = new Mock<IResetPasswordTokenRepository>();
        private readonly Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();
        private readonly Mock<IPasswordService> _mockPasswordService = new Mock<IPasswordService>();

        private readonly ResetUserPasswordService _resetUserPasswordService;
        private readonly UpdateResetTokenPasswordHandler _handler;

        public UpdateResetTokenPasswordCommandTests()
        {
            _resetUserPasswordService = new ResetUserPasswordService(_mockUserRepository.Object,
                _mockTokenRepository.Object,
                _mockUnitOfWork.Object,
                _mockPasswordService.Object);

            _handler = new UpdateResetTokenPasswordHandler(_resetUserPasswordService,
                _mockLogger.Object);
        }

        [Fact(DisplayName = "Удачный сброс пароля")]
        public async Task Handle_ShouldReturnSuccess_WhenUserResetTokenUpdateSuccessfully()
        {
            var email = "test@local.com";

            var user = User.Create(new EmailVo(email),
                new FullNameVo("John", "Doe", "Smith"),
                "hashedPassword",
                UserTypes.Standart,
                UserStatuses.Inactive
            );

            var token = UserResetPasswordToken.Create(user.Id);

            var request = new UpdateResetTokenPasswordCommand(new UserResetPasswordRequest() { Password = "123321", Token = token.Id });

            _mockUserRepository.Setup(x => x.GetByIdAsync(user.Id, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _mockTokenRepository.Setup(x => x.GetByIdAsync(token.Id, It.IsAny<CancellationToken>())).ReturnsAsync(token);
            _mockPasswordService.Setup(x => x.GetHashPassword("123321")).Returns("hashedPassword");

            // Act
            var result = await _handler.Handle(request, It.IsAny<CancellationToken>());

            // Assert
            Assert.True(result.IsSuccess);
            _mockUnitOfWork.Verify(u => u.BeginTransactionAsync(CancellationToken.None), Times.Once);
            _mockUnitOfWork.Verify(u => u.CommitAsync(CancellationToken.None), Times.Once);
            _mockUserRepository.Verify(r => r.Update(It.IsAny<User>()), Times.Once);
            _mockTokenRepository.Verify(r => r.Update(It.IsAny<UserResetPasswordToken>()), Times.Once);
        }

        [Fact(DisplayName = "Неудачная попытка сброса пароля, при истекшем токене")]
        public async Task Handle_ShouldReturnException_WhenUserResetTokenExpired()
        {
            // Arrange
            var email = "test@local.com";
            var user = User.Create(new EmailVo(email),
                new FullNameVo("John", "Doe", "Smith"),
                "hashedPassword",
                UserTypes.Standart,
                UserStatuses.Inactive
            );

            var token = UserResetPasswordToken.Create(user.Id);
            token.MarkAsExpired();

            var request = new UpdateResetTokenPasswordCommand(new UserResetPasswordRequest() { Password = "123321", Token = token.Id });

            _mockUserRepository.Setup(x => x.GetByIdAsync(user.Id, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _mockTokenRepository.Setup(x => x.GetByIdAsync(token.Id, It.IsAny<CancellationToken>())).ReturnsAsync(token);
            _mockPasswordService.Setup(x => x.GetHashPassword("123321")).Returns("hashedPassword");

            // Act
            var result = await _handler.Handle(request, It.IsAny<CancellationToken>());

            // Assert
            Assert.False(result.IsSuccess);
            _mockUnitOfWork.Verify(u => u.BeginTransactionAsync(CancellationToken.None), Times.Once);
            _mockUnitOfWork.Verify(u => u.RollbackAsync(CancellationToken.None), Times.Once);
            Assert.Equal(ServicesUsersExceptionCodes.ServicesUsersExceptionExpiredToken, result.Error.Code);
        }

        [Fact(DisplayName = "Неудачная попытка сброса пароля, при использованном токене")]
        public async Task Handle_ShouldReturnException_WhenUserResetTokenUsed()
        {
            // Arrange
            var email = "test@local.com";
            var user = User.Create(new EmailVo(email),
                new FullNameVo("John", "Doe", "Smith"),
                "hashedPassword",
                UserTypes.Standart,
                UserStatuses.Inactive
            );

            var token = UserResetPasswordToken.Create(user.Id);
            token.MarkAsUsed();

            var request = new UpdateResetTokenPasswordCommand(new UserResetPasswordRequest() { Password = "123321", Token = token.Id });

            _mockUserRepository.Setup(x => x.GetByIdAsync(user.Id, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _mockTokenRepository.Setup(x => x.GetByIdAsync(token.Id, It.IsAny<CancellationToken>())).ReturnsAsync(token);
            _mockPasswordService.Setup(x => x.GetHashPassword("123321")).Returns("hashedPassword");

            // Act
            var result = await _handler.Handle(request, It.IsAny<CancellationToken>());

            // Assert
            Assert.False(result.IsSuccess);
            _mockUnitOfWork.Verify(u => u.BeginTransactionAsync(CancellationToken.None), Times.Once);
            _mockUnitOfWork.Verify(u => u.RollbackAsync(CancellationToken.None), Times.Once);
            Assert.Equal(ServicesUsersExceptionCodes.ServicesUsersExceptionUsedToken, result.Error.Code);
        }
    }
}
