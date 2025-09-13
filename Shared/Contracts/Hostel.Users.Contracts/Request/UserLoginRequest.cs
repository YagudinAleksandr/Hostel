namespace Hostel.Users.Contracts.Request
{
    /// <summary>
    /// Запрос на авторизацию пользователя
    /// </summary>
    public class UserLoginRequest
    {
        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        public string Email { get; set; } = null!;

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; } = null!;
    }
}
