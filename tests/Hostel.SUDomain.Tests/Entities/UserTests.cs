using Hostel.SU.Domain;
using Hostel.Domain.Primitives;

namespace Hostel.SUDomain.Tests.Entities
{
    /// <summary>
    /// Тесты на агрегат <see cref="User"/>
    /// </summary>
    public class UserTests
    {
        [Fact(DisplayName = "Создание пользователя")]
        public async Task Should_Create_User()
        {
            // Act
            var user = User.Create(new EmailVo("test@test.local"),
                new FullNameVo("Test", "Test", "Test"),
                "passwordHash", UserTypes.Standart,
                UserStatuses.Inactive);

            // Assert
            Assert.NotNull(user);
            Assert.Equal("Test", user.Name.FirstName);
        }
    }
}
