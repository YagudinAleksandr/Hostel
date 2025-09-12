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
    /// Тесты на обработчик команды <see cref="UpdateUserFullnameHandler"/>
    /// </summary>
    public class UpdateUserFullnameCommandTests
    {
        private readonly Mock<IUserRepository> _mockRepo;
        private readonly Mock<IUnitOfWork> _mockUow;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<UpdateUserFullnameHandler>> _mockLogger;

        private readonly UpdateUserFullnameHandler _handler;

        public UpdateUserFullnameCommandTests()
        {
            _mockRepo = new Mock<IUserRepository>();
            _mockUow = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<UpdateUserFullnameHandler>>();

            _handler = new UpdateUserFullnameHandler(_mockRepo.Object, _mockUow.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [Fact(DisplayName = "Удачное обновление ФИО пользователя")]
        public async Task Handle_ShouldReturnSuccess_WhenUserUpdateFullnameSuccessfully()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var request = new UpdateUserFullnameCommand(new UserUpdateFullnameRequest()
            {
                Id = userId,
                Lastname = "Last name 2",
                Firstname = "First name 2",
                Patronymic = "Patronymic 2"
            });

            _mockRepo.Setup(r => r.GetByIdAsync(userId, default)).ReturnsAsync(User.Create(
                new EmailVo("test@example.com"),
                new FullNameVo("First name", "Last name", "Patronymic"),
                "hashedPassword",
                UserTypes.Standart,
                UserStatuses.Active
            ));

            _mockMapper.Setup(m => m.Map<UserResponse>(It.IsAny<User>())).Returns(new UserResponse
            {
                Id = userId,
                Email = "test2@example.com",
                Firstname = "First name 2",
                Lastname = "Last name 2",
                Patronymic = "Patronymic 2",
                Type = "STANDART",
                Status = "ACTIVE"
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
