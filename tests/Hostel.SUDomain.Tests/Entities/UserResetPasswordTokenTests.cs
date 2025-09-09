using FluentAssertions;
using Hostel.SU.Domain;

namespace Hostel.SUDomain.Tests.Entities
{
    /// <summary>
    /// Тесты на агрегат <see cref="UserResetPasswordToken"/>
    /// </summary>
    public class UserResetPasswordTokenTests
    {
        [Fact(DisplayName = "Создание токена сброса пароля")]
        public async Task Should_Create_Reset_Token()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            var result = UserResetPasswordToken.Create(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
        }

        [Fact(DisplayName = "Изменяет статус токена на Использован")]
        public async Task Should_Mark_As_Used()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var token = UserResetPasswordToken.Create(userId);

            // Act
            token.MarkAsUsed();

            // Assert
            Assert.Equal(userId, token.UserId);
            Assert.Equal(ServicesUsersResetPasswordStatusCodes.Used, token.Status.Code);
        }

        [Fact(DisplayName = "Невозможно изменить статус токена на Использован, если он был уже использован")]
        public async Task Should_Exception_To_Change_On_Used()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var token = UserResetPasswordToken.Create(userId);

            // Act
            token.MarkAsUsed();

            var res = () => token.MarkAsUsed();

            // Assert
            res.Should().Throw<DomainUsedTokenException>();
        }

        [Fact(DisplayName = "Невозможно изменить статус токена на Использован, если он истек")]
        public async Task Should_Exception_To_Change_On_Expired()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var token = UserResetPasswordToken.Create(userId);

            // Act
            token.MarkAsExpired();

            var res = () => token.MarkAsUsed();

            // Assert
            res.Should().Throw<DomainExpiredTokenException>();
        }
    }
}
