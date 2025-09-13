using Hostel.SU.Domain;
using Hostel.Domain.Primitives;
using FluentAssertions;
using Hostel.Shared.Kernel;

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

            await Task.CompletedTask;
        }

        [Fact(DisplayName = "Должен вернуть исключение об обязательном заполнении поля")]
        public async Task Should_Return_Required_Field_Exception()
        {
            // Act
            var user = () => User.Create(new EmailVo("test@test.local"),
                new FullNameVo("Test", "Test", "Test"),
                "", UserTypes.Standart,
                UserStatuses.Inactive);

            // Assert
            user.Should().Throw<DomainRequiredFieldException>();

            await Task.CompletedTask;
        }

        [Fact(DisplayName = "Смена ФИО пользователя")]
        public async Task Should_Change_Name()
        {
            // Arrange
            var user = CreateTestUser(UserStatuses.Active);

            // Act
            user.ChangeName(new FullNameVo("TestChange", "Test", "Test"));

            // Arrange
            Assert.NotNull(user);
            Assert.NotEqual("Test", user.Name.FirstName);

            await Task.CompletedTask;
        }

        [Theory(DisplayName = "Невозможно изменить ФИО, если пользователь не активен")]
        [InlineData(ServicesUsersStatusesCodes.ServicesUsersUserStatusInactiveCode)]
        [InlineData(ServicesUsersStatusesCodes.ServicesUsersUserStatusBlockedCode)]
        public async Task Should_Not_Change_Name_When_User_Not_Active(string status)
        {
            // Arrange
            var userStatus = UserStatuses.All.FirstOrDefault(x => x.Code == status);
            var user = CreateTestUser(userStatus);

            // Act
            var res = () => user.ChangeName(new FullNameVo("Test", "Test", "Test"));

            // Assert
            res.Should().Throw<DomainInactiveUserException>();

            await Task.CompletedTask;
        }

        [Fact(DisplayName = "Смена адрес электронной почты пользователя")]
        public async Task Should_Change_Email()
        {
            // Arrange
            var user = CreateTestUser(UserStatuses.Active);

            // Act
            user.ChangeEmail(new EmailVo("test@test2.local"));

            // Arrange
            Assert.NotNull(user);
            Assert.Equal("test@test2.local", user.Email.Value);

            await Task.CompletedTask;
        }

        [Theory(DisplayName = "Невозможно изменить адрес электронной почты, если пользователь не активен")]
        [InlineData(ServicesUsersStatusesCodes.ServicesUsersUserStatusInactiveCode)]
        [InlineData(ServicesUsersStatusesCodes.ServicesUsersUserStatusBlockedCode)]
        public async Task Should_Not_Change_Email_When_User_Not_Active(string status)
        {
            // Arrange
            var userStatus = UserStatuses.All.FirstOrDefault(x => x.Code == status);
            var user = CreateTestUser(userStatus);

            // Act
            var res = () => user.ChangeEmail(new EmailVo("test@test2.local"));

            // Assert
            res.Should().Throw<DomainInactiveUserException>();

            await Task.CompletedTask;
        }

        [Fact(DisplayName = "Смена пароля пользователя")]
        public async Task Should_Change_Password()
        {
            // Arrange
            var user = CreateTestUser(UserStatuses.Active);

            // Act
            user.ChangePassword("123321");

            // Arrange
            Assert.NotNull(user);
            Assert.Equal("123321", user.PasswordHash);

            await Task.CompletedTask;
        }

        [Theory(DisplayName = "Невозможно изменить пароль, если пользователь не активен")]
        [InlineData(ServicesUsersStatusesCodes.ServicesUsersUserStatusInactiveCode)]
        [InlineData(ServicesUsersStatusesCodes.ServicesUsersUserStatusBlockedCode)]
        public async Task Should_Not_Change_Password_When_User_Not_Active(string status)
        {
            // Arrange
            var userStatus = UserStatuses.All.FirstOrDefault(x => x.Code == status);
            var user = CreateTestUser(userStatus);

            // Act
            var res = () => user.ChangePassword("123321");

            // Assert
            res.Should().Throw<DomainInactiveUserException>();

            await Task.CompletedTask;
        }

        [Fact(DisplayName = "Смена типа пользователя")]
        public async Task Should_Change_Type()
        {
            // Arrange
            var user = CreateTestUser(UserStatuses.Active);

            // Act
            user.ChangeType(UserTypes.Manager);

            // Arrange
            Assert.NotNull(user);
            Assert.Equal(ServicesUsersTypeCodes.Manager, user.Type.Code);

            await Task.CompletedTask;
        }

        [Theory(DisplayName = "Невозможно изменить тип пользователя, если пользователь не активен")]
        [InlineData(ServicesUsersStatusesCodes.ServicesUsersUserStatusInactiveCode)]
        [InlineData(ServicesUsersStatusesCodes.ServicesUsersUserStatusBlockedCode)]
        public async Task Should_Not_Change_Type_When_User_Not_Active(string status)
        {
            // Arrange
            var userStatus = UserStatuses.All.FirstOrDefault(x => x.Code == status);
            var user = CreateTestUser(userStatus);

            // Act
            var res = () => user.ChangeType(UserTypes.Manager);

            // Assert
            res.Should().Throw<DomainInactiveUserException>();

            await Task.CompletedTask;
        }

        [Fact(DisplayName = "Смена статуса пользователя")]
        public async Task Should_Change_Status()
        {
            // Arrange
            var user = CreateTestUser(UserStatuses.Active);

            // Act
            user.ChangeStatus(UserStatuses.Blocked);

            // Arrange
            Assert.NotNull(user);
            Assert.Equal(ServicesUsersStatusesCodes.ServicesUsersUserStatusBlockedCode, user.Status.Code);

            await Task.CompletedTask;
        }

        /// <summary>
        /// Создание тестового пользователя
        /// </summary>
        /// <param name="status">Статус активности</param>
        /// <returns></returns>
        private User CreateTestUser(UserStatus status)
        {
            return User.Create(new EmailVo("test@test.local"),
                new FullNameVo("Test", "Test", "Test"),
                "123654548154", UserTypes.Standart,
               status);
        }
    }
}
