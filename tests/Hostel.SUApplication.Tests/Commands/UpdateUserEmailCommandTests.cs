using AutoMapper;
using Hostel.Shared.Kernel;
using Hostel.SU.Application;
using Hostel.SU.Domain;
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
    }
}
