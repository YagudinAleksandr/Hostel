namespace Hostel.Users.Contracts.Request
{
    /// <summary>
    /// Запрос на обновление пароля по токену
    /// </summary>
    public record UserResetPasswordRequest
    {
        /// <summary>
        /// Новый пароль
        /// </summary>
        public string Password { get; set; } = null!;

        /// <summary>
        /// Токен сброса пароля
        /// </summary>
        public Guid Token { get; set; }
    }
}
