using AutoMapper;
using Hostel.SU.Application;
using Hostel.SU.Domain;
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

        [Fact(DisplayName ="Проверка что токен не истек и не отозван должна вернуть false")]
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
    }
}
