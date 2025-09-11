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
    /// Тесты для обработчика команды <see cref="UpdateUserEmailHandler"/>
    /// </summary>
    public class UpdateUserEmailCommandTests
    {
        private readonly Mock<IUserRepository> _mockRepo;
        private readonly Mock<IUnitOfWork> _mockUow;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<UpdateUserEmailHandler>> _mockLogger;

        private readonly UpdateUserEmailHandler _handler;

        public UpdateUserEmailCommandTests()
        {
            _mockRepo = new Mock<IUserRepository>();
            _mockUow = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<UpdateUserEmailHandler>>();

            _handler = new UpdateUserEmailHandler(_mockRepo.Object, _mockUow.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [Fact(DisplayName = "Удачное обновление адреса электронной почты")]
        public async Task Handle_ShouldReturnSuccess_WhenUserUpdateEmailSuccessfully()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var request = new UpdateUserEmailCommand(new UserUpdateEmailRequest()
            {
                Id = userId,
                Email = "test2@example.com"
            });

            _mockRepo.Setup(r => r.GetByIdAsync(userId, default)).ReturnsAsync(User.Create(
                new EmailVo("test@example.com"),
                new FullNameVo("John", "Doe", "Smith"),
                "hashedPassword",
                UserTypes.Standart,  // мокаем первый тип
                UserStatuses.Inactive // мокаем первый статус
            ));

            _mockRepo.Setup(r => r.IsEmailExists("test@example.com", userId)).Returns(false);
            _mockMapper.Setup(m => m.Map<UserResponse>(It.IsAny<User>())).Returns(new UserResponse
            {
                Id = userId,
                Email = "test2@example.com",
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
            _mockRepo.Verify(r => r.Update(It.IsAny<User>()), Times.Once);
        }
    }
}
