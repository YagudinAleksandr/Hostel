namespace Hostel.Users.Contracts.Request
{
    /// <summary>
    /// Запрос на изменение пароля пользователя
    /// </summary>
    public record UserUpdatePasswordRequest
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Старый пароль
        /// </summary>
        public string OldPassword { get; set; } = null!;

        /// <summary>
        /// Новый пароль
        /// </summary>
        public string NewPassword { get; set; } = null!;

        /// <summary>
        /// Подтверждение нового пароля
        /// </summary>
        public string ConfirmedNewPassword { get; set; } = null!;
    }
}
