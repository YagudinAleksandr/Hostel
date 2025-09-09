using Hostel.SU.Domain;

namespace Hostel.SUDomain.Tests.Entities
{
    /// <summary>
    /// Тесты на агрегат <see cref="RefreshToken"/>
    /// </summary>
    public class RefreshTokenTests
    {
        [Fact(DisplayName = "Создание токена обновления")]
        public async Task Should_Create_Refresh_Token()
        {
            // Act
            var token = new RefreshToken("testToken", DateTime.UtcNow.AddDays(1), Guid.NewGuid());

            // Assert
            Assert.NotNull(token);
            Assert.True(token.IsActive);
        }

        [Fact(DisplayName = "Проверка валидности токена")]
        public async Task Should_Not_Active()
        {
            // Act
            var token = new RefreshToken("testToken", DateTime.UtcNow.AddDays(-1), Guid.NewGuid());

            // Assert
            Assert.NotNull(token);
            Assert.False(token.IsActive);
        }

        [Fact(DisplayName = "Отзыв токена")]
        public async Task Should_Revoke_Token()
        {
            // Arrange
            var token = new RefreshToken("testToken", DateTime.UtcNow.AddDays(1), Guid.NewGuid());

            // Act
            token.Revoke();

            // Assert
            Assert.NotNull(token);
            Assert.False(token.IsActive);
        }
    }
}
