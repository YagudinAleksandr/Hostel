using AutoMapper;
using Hostel.Domain.Primitives;
using Hostel.SU.Application;
using Hostel.SU.Domain;
using Hostel.Users.Contracts.Request;
using Hostel.Users.Contracts.Response;
using Microsoft.Extensions.Logging;
using Moq;

namespace Hostel.SUApplication.Tests.Queries
{
    /// <summary>
    /// Тесты на обработчик запроса <see cref="AuthUserHandler"/>
    /// </summary>
    public class AuthUserQueryTests
    {
        private readonly Mock<ILogger<AuthUserHandler>> _mockLogger;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IUserRepository> _mockUserRepo;
        private readonly Mock<ITokenGeneratorService> _mockTokenGeneratorService;
        private readonly Mock<IPasswordService> _mockPasswordService;
        private readonly Mock<IRefreshTokenRepository> _mockTokenRefreshRepository;

        private readonly AuthService _authService;

        private readonly AuthUserHandler _handler;

        public AuthUserQueryTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockUserRepo = new Mock<IUserRepository>();
            _mockPasswordService = new Mock<IPasswordService>();
            _mockTokenGeneratorService = new Mock<ITokenGeneratorService>();
            _mockTokenRefreshRepository = new Mock<IRefreshTokenRepository>();

            _authService = new AuthService(_mockUserRepo.Object,
                _mockTokenRefreshRepository.Object,
                _mockTokenGeneratorService.Object,
                _mockPasswordService.Object,
                _mockMapper.Object);

            _mockLogger = new Mock<ILogger<AuthUserHandler>>();

            _handler = new AuthUserHandler(_authService, _mockLogger.Object);
        }

        [Fact(DisplayName = "Удачная авторизация")]
        public async Task Handle_ShouldReturnSuccess_WhenUserAuthSuccessfully()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new AuthUserQuery(new UserLoginRequest() { Email = "test@test.local", Password = "password123" });

            _mockUserRepo.Setup(g => g.GetByEmailAsync(request.Login.Email, default)).ReturnsAsync(User.Create(
                new EmailVo("test@test.local"),
                new FullNameVo("John", "Doe", "Smith"),
                "hashedPassword",
                UserTypes.Standart,  // мокаем первый тип
                UserStatuses.Active // мокаем первый статус
            ));

            _mockPasswordService.Setup(g => g.ValidateHash(request.Login.Password, "hashedPassword")).Returns(true);

            _mockTokenGeneratorService.Setup(g => g.GenerateAccessToken(It.IsAny<Guid>(), UserTypes.Standart)).Returns("access_token");
            _mockTokenGeneratorService.Setup(g => g.GenerateRefreshToken(It.IsAny<Guid>())).Returns("refresh_token");
            _mockTokenGeneratorService.Setup(g => g.RefreshTokenLifetimeMinutes).Returns(15);

            _mockMapper.Setup(m => m.Map<UserResponse>(It.IsAny<User>())).Returns(new UserResponse
            {
                Id = userId,
                Email = "test@example.com",
                Firstname = "John",
                Lastname = "Doe",
                Patronymic = "Smith",
                Type = "ADMINISTRATOR",
                Status = "ACTIVE"
            });

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            _mockTokenRefreshRepository.Verify(u => u.AddAsync(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
