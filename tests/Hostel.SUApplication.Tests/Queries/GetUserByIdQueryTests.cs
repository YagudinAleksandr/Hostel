using AutoMapper;
using Hostel.Domain.Primitives;
using Hostel.Shared.Kernel;
using Hostel.SU.Application;
using Hostel.SU.Domain;
using Hostel.Users.Contracts.Response;
using Microsoft.Extensions.Logging;
using Moq;

namespace Hostel.SUApplication.Tests.Queries
{
    /// <summary>
    /// Тесты на запрос <see cref="GetUserByIdHandler"/>
    /// </summary>
    public class GetUserByIdQueryTests
    {
        private readonly Mock<ILogger<GetUserByIdHandler>> _mockLogger = new Mock<ILogger<GetUserByIdHandler>>();
        private readonly Mock<IMapper> _mockMapper = new Mock<IMapper>();
        private readonly Mock<IUserRepository> _mockUserRepo = new Mock<IUserRepository>();

        private readonly GetUserByIdHandler _handler;

        public GetUserByIdQueryTests()
        {
            _handler = new GetUserByIdHandler(_mockUserRepo.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [Fact(DisplayName = "Удачное получение пользователя по идентификатору")]
        public async Task Handle_ShouldReturnSuccess_WhenUserFindById()
        {
            // Arrange
            var email = "test@local.com";

            var user = User.Create(new EmailVo(email),
                new FullNameVo("John", "Doe", "Smith"),
                "hashedPassword",
                UserTypes.Standart,
                UserStatuses.Inactive
            );

            var request = new GetUserByIdQuery(user.Id);

            _mockUserRepo.Setup(r => r.GetByIdAsync(user.Id, It.IsAny<CancellationToken>())).ReturnsAsync(user);
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
            var result = await _handler.Handle(request, It.IsAny<CancellationToken>());

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
        }

        [Fact(DisplayName = "Ошибка, если пользователь не найден")]
        public async Task Handle_ShouldReturnException_WhenUserNotFindById()
        {
            // Arrange
            var email = "test@local.com";

            var user = User.Create(new EmailVo(email),
                new FullNameVo("John", "Doe", "Smith"),
                "hashedPassword",
                UserTypes.Standart,
                UserStatuses.Inactive
            );

            var request = new GetUserByIdQuery(user.Id);

            _mockUserRepo.Setup(r => r.GetByIdAsync(user.Id, It.IsAny<CancellationToken>())).ReturnsAsync(default(User));

            // Act
            var result = await _handler.Handle(request, It.IsAny<CancellationToken>());

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(DomainExceptionCodes.DomainResourceNotFoundWithParam, result.Error.Code);
        }
    }
}
