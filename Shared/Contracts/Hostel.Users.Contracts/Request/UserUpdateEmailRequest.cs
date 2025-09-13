namespace Hostel.Users.Contracts.Request
{
    /// <summary>
    /// Запрос на обновлени адреса электронной почты
    /// </summary>
    public record UserUpdateEmailRequest
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        public string Email { get; set; } = null!;
    }
}
