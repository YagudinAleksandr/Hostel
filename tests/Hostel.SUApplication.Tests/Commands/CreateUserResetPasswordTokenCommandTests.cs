using Hostel.Domain.Primitives;
using Hostel.Shared.Kernel;
using Hostel.SU.Application;
using Hostel.SU.Domain;
using Microsoft.Extensions.Logging;
using Moq;

namespace Hostel.SUApplication.Tests.Commands
{
    /// <summary>
    /// Тесты на команду <see cref="CreateUserResetPasswordTokenHandler"/>
    /// </summary>
    public class CreateUserResetPasswordTokenCommandTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository = new Mock<IUserRepository>();
        private readonly Mock<ILogger<CreateUserResetPasswordTokenHandler>> _mockLogger = new Mock<ILogger<CreateUserResetPasswordTokenHandler>>();

        private readonly Mock<IResetPasswordTokenRepository> _mockTokenRepository = new Mock<IResetPasswordTokenRepository>();
        private readonly Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();
        private readonly Mock<IPasswordService> _mockPasswordService = new Mock<IPasswordService>();

        private readonly ResetUserPasswordService _resetUserPasswordService;
        private readonly CreateUserResetPasswordTokenHandler _handler;

        public CreateUserResetPasswordTokenCommandTests()
        {
            _resetUserPasswordService = new ResetUserPasswordService(_mockUserRepository.Object,
                _mockTokenRepository.Object,
                _mockUnitOfWork.Object,
                _mockPasswordService.Object);

            _handler = new CreateUserResetPasswordTokenHandler(_resetUserPasswordService,
                _mockUserRepository.Object,
                _mockLogger.Object);
        }

        [Fact(DisplayName = "Успешное создание токена для сброса пароля")]
        public async Task Handle_ShouldReturnSuccess_WhenUserResetTokenCreateSuccessfully()
        {
            // Arrange
            var email = "test@local.com";

            var user = User.Create(new EmailVo(email),
                new FullNameVo("John", "Doe", "Smith"),
                "hashedPassword",
                UserTypes.Standart,
                UserStatuses.Inactive
            );

            var request = new CreateUserResetPasswordTokenCommand(email);

            _mockUserRepository.Setup(x => x.GetByEmailAsync(email, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _mockUserRepository.Setup(x => x.GetByIdAsync(user.Id, It.IsAny<CancellationToken>())).ReturnsAsync(user);

            // Act
            var result = await _handler.Handle(request, It.IsAny<CancellationToken>());

            // Assert
            Assert.True(result.IsSuccess);
            _mockUnitOfWork.Verify(u => u.BeginTransactionAsync(CancellationToken.None), Times.Once);
            _mockUnitOfWork.Verify(u => u.CommitAsync(CancellationToken.None), Times.Once);
            _mockUserRepository.Verify(r => r.Update(It.IsAny<User>()), Times.Once);
            _mockTokenRepository.Verify(r=>r.Add(It.IsAny<UserResetPasswordToken>()), Times.Once);
        }

        [Fact(DisplayName = "Неудачная попытка создания токена, если пользователь не найден по идентификатору")]
        public async Task Handle_ShouldReturnException_WhenUserResetTokenCreateWithNotFoundedUserById()
        {
            // Arrange
            var email = "test@local.com";

            var user = User.Create(new EmailVo(email),
                new FullNameVo("John", "Doe", "Smith"),
                "hashedPassword",
                UserTypes.Standart,
                UserStatuses.Inactive
            );

            var request = new CreateUserResetPasswordTokenCommand(email);

            _mockUserRepository.Setup(x => x.GetByEmailAsync(email, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _mockUserRepository.Setup(x => x.GetByIdAsync(user.Id, It.IsAny<CancellationToken>())).ReturnsAsync(default(User));

            // Act
            var result = await _handler.Handle(request, It.IsAny<CancellationToken>());

            // Assert
            Assert.False(result.IsSuccess);
            _mockUnitOfWork.Verify(u => u.BeginTransactionAsync(CancellationToken.None), Times.Once);
            _mockUnitOfWork.Verify(u => u.RollbackAsync(CancellationToken.None), Times.Once);
            Assert.Equal(DomainExceptionCodes.DomainResourceNotFoundWithParam, result.Error.Code);
        }
    }
}
