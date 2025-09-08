using Hostel.SU.Domain;

namespace Hostel.SUDomain.Tests.VO
{
    public class EmailVoTests
    {
        [Fact(DisplayName = "Создание адреса электронной почты")]
        public async Task Should_Create_Email()
        {
            // Arrange
            string email = "test@test.local";

            // Act
            var result = new EmailVo(email);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(email, result.Value);
        }
    }
}
