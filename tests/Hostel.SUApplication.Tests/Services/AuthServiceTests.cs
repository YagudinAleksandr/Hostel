using AutoMapper;
using FluentAssertions;
using Hostel.Shared.Kernel;
using Hostel.SU.Application;
using Hostel.SU.Domain;
using Hostel.Users.Contracts.Response;
using Moq;

namespace Hostel.SUApplication.Tests.Services
{
    /// <summary>
    /// Тесты на сервис <see cref="AuthService"/>
    /// </summary>
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserReposytory = new Mock<IUserRepository>();
        private readonly Mock<IRefreshTokenRepository> _mockRefreshTokenRepository = new Mock<IRefreshTokenRepository>();
        private readonly Mock<ITokenGeneratorService> _mockTokenGeneratorService = new Mock<ITokenGeneratorService>();
        private readonly Mock<IPasswordService> _mockPasswordsService = new Mock<IPasswordService>();
        private readonly Mock<IMapper> _mockMapper = new Mock<IMapper>();

        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _authService = new AuthService(_mockUserReposytory.Object,
                _mockRefreshTokenRepository.Object,
                _mockTokenGeneratorService.Object,
                _mockPasswordsService.Object,
                _mockMapper.Object);
        }

        [Fact(DisplayName = "Проверка что токен не истек и не отозван должна вернуть false")]
        public async Task ShouldReturnFalse_IsTokenRevokedAsync_Success()
        {
            // Arrange
            string token = "refresh_token";
            var refreshToken = new RefreshToken(token, DateTime.UtcNow.AddDays(1), Guid.NewGuid());

            _mockRefreshTokenRepository.Setup(x => x.GetByTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(refreshToken);

            // Act
            var result = await _authService.IsTokenRevokedAsync(token);

            // Assert
            Assert.False(result);
        }

        [Fact(DisplayName = "Проверка что токен не истек и не отозван должна вернуть true, когда токен не найден")]
        public async Task ShouldReturnTrue_IsTokenRevokedAsync_TokenNotFound()
        {
            // Arrange
            string token = "refresh_token";

            _mockRefreshTokenRepository.Setup(x => x.GetByTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(default(RefreshToken));

            // Act
            var result = await _authService.IsTokenRevokedAsync(token);

            // Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Проверка что токен не истек и не отозван должна вернуть true, когда токен не активен")]
        public async Task ShouldReturnTrue_IsTokenRevokedAsync_TokenNotActive()
        {
            // Arrange
            string token = "refresh_token";
            var refreshToken = new RefreshToken(token, DateTime.UtcNow.AddDays(1), Guid.NewGuid());
            refreshToken.Revoke();

            _mockRefreshTokenRepository.Setup(x => x.GetByTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(refreshToken);


            // Act
            var result = await _authService.IsTokenRevokedAsync(token);

            // Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Проверка что токен не истек и не отозван должна вернуть true, когда токен не истек")]
        public async Task ShouldReturnTrue_IsTokenRevokedAsync_TokenExpired()
        {
            // Arrange
            string token = "refresh_token";
            var refreshToken = new RefreshToken(token, DateTime.UtcNow.AddDays(-1), Guid.NewGuid());

            _mockRefreshTokenRepository.Setup(x => x.GetByTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(refreshToken);


            // Act
            var result = await _authService.IsTokenRevokedAsync(token);

            // Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Отзыв токена, должен сохранить в репозиторий")]
        public async Task ShouldSave_TokenRevokeAsync_Success()
        {
            // Arrange
            string token = "refresh_token";
            var refreshToken = new RefreshToken(token, DateTime.UtcNow.AddDays(1), Guid.NewGuid());

            _mockRefreshTokenRepository.Setup(x => x.GetByTokenAsync(token, It.IsAny<CancellationToken>())).ReturnsAsync(refreshToken);

            // Act
            await _authService.RevokeTokenAsync(token, It.IsAny<CancellationToken>());

            // Assert
            _mockRefreshTokenRepository.Verify(x => x.UpdateAsync(refreshToken, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Отзыв токена, вернуть исключение")]
        public async Task ShouldSave_TokenRevokeAsync_ExceptionRised()
        {
            // Arrange
            string token = "refresh_token";

            _mockRefreshTokenRepository.Setup(x => x.GetByTokenAsync(token, It.IsAny<CancellationToken>())).ReturnsAsync(default(RefreshToken));

            // Act
            var act = async () => await _authService.RevokeTokenAsync(token, It.IsAny<CancellationToken>());

            // Assert
            await act.Should().ThrowAsync<DomainResourceNotFoundException>();
        }

        [Fact(DisplayName = "Формирование токена после успешной авторизации должен вернуть удачный ответ")]
        public async Task ShouldReturnTokenResponse_LoginAsync_Success()
        {
            // Arrange
            string email = "test@test.com";
            string password = "password";
            var user = User.Create(new EmailVo(email), new Domain.Primitives.FullNameVo("Тест", "Тест", ""), "password", UserTypes.Standart, UserStatuses.Active);

            _mockUserReposytory.Setup(x => x.GetByEmailAsync(email, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _mockPasswordsService.Setup(x => x.ValidateHash(password, It.IsAny<string>())).Returns(true);
            _mockTokenGeneratorService.Setup(x => x.GenerateAccessToken(user.Id, user.Type)).Returns("access_token");
            _mockTokenGeneratorService.Setup(x => x.GenerateRefreshToken(user.Id)).Returns("access_token");
            _mockTokenGeneratorService.Setup(x => x.RefreshTokenLifetimeMinutes).Returns(30);
            _mockMapper.Setup(m => m.Map<UserResponse>(It.IsAny<User>())).Returns(new UserResponse
            {
                Id = user.Id,
                Email = email,
                Firstname = "John",
                Lastname = "Doe",
                Patronymic = "Smith",
                Type = "ADMINISTRATOR",
                Status = "ACTIVE"
            });

            // Act
            var result = await _authService.LoginAsync(email, password, It.IsAny<CancellationToken>());

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.User);
            Assert.Equal(email, result.User.Email);
            _mockRefreshTokenRepository.Verify(x => x.AddAsync(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Формирование токена должно вернуть исключение, если пользователь не найден")]
        public async Task ShouldReturnException_LoginAsync_WhenUserNotFound()
        {
            // Arrange
            string email = "test@test.com";
            string password = "password";

            _mockUserReposytory.Setup(x => x.GetByEmailAsync(email, It.IsAny<CancellationToken>())).ReturnsAsync(default(User));

            // Act
            var result = async () => await _authService.LoginAsync(email, password, It.IsAny<CancellationToken>());

            // Assert
            await result.Should().ThrowAsync<DomainResourceNotFoundException>();
        }

        [Fact(DisplayName = "Формирование токена должно вернуть исключение, если пароль не совпал")]
        public async Task ShouldReturnException_LoginAsync_WhenUserPasswordNotCorrect()
        {
            // Arrange
            string email = "test@test.com";
            string password = "password";
            var user = User.Create(new EmailVo(email), new Domain.Primitives.FullNameVo("Тест", "Тест", ""), "password", UserTypes.Standart, UserStatuses.Active);

            _mockUserReposytory.Setup(x => x.GetByEmailAsync(email, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _mockPasswordsService.Setup(x => x.ValidateHash(password, It.IsAny<string>())).Returns(false);

            // Act
            var result = async () => await _authService.LoginAsync(email, password, It.IsAny<CancellationToken>());

            // Assert
            await result.Should().ThrowAsync<DomainResourceNotFoundException>();
        }

        [Fact(DisplayName = "Формирование токена должно вернуть исключение, если пользователь не активен")]
        public async Task ShouldReturnException_LoginAsync_WhenUserStatusNotActive()
        {
            // Arrange
            string email = "test@test.com";
            string password = "password";
            var user = User.Create(new EmailVo(email), new Domain.Primitives.FullNameVo("Тест", "Тест", ""), "password", UserTypes.Standart, UserStatuses.Active);
            user.ChangeStatus(UserStatuses.Inactive);

            _mockUserReposytory.Setup(x => x.GetByEmailAsync(email, It.IsAny<CancellationToken>())).ReturnsAsync(user);

            // Act
            var result = async () => await _authService.LoginAsync(email, password, It.IsAny<CancellationToken>());

            // Assert
            await result.Should().ThrowAsync<DomainInactiveUserException>();
        }
    }
}
