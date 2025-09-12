namespace Hostel.Users.Contracts.Response
{
    /// <summary>
    /// Ответ от авторизации
    /// </summary>
    public class UserLoginResponse
    {
        /// <summary>
        /// Токен доступа
        /// </summary>
        public string AccessToken { get; set; } = null!;

        /// <summary>
        /// Токен обновления
        /// </summary>
        public string RefreshToken { get; set; } = null!;

        /// <summary>
        /// Пользователь
        /// </summary>
        public UserResponse User { get; set; } = null!;
    }
}
