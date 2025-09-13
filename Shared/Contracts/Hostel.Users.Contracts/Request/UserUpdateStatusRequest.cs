namespace Hostel.Users.Contracts.Request
{
    /// <summary>
    /// Запрос на изменение статуса пользователя
    /// </summary>
    public record UserUpdateStatusRequest
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Код статуса
        /// </summary>
        public string StatusCode { get; set; } = null!;
    }
}
