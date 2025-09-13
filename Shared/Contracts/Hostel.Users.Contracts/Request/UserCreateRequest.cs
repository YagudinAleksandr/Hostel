namespace Hostel.Users.Contracts.Request
{
    /// <summary>
    /// Запрос на создание пользователя
    /// </summary>
    public record UserCreateRequest
    {
        /// <summary>
        /// Фамилия
        /// </summary>
        public string Lastname { get; set; } = null!;

        /// <summary>
        /// Имя
        /// </summary>
        public string Firstname { get; set; } = null!;

        /// <summary>
        /// Отчесвто
        /// </summary>
        public string? Patronymic { get; set; }

        /// <summary>
        /// Тип
        /// </summary>
        public string Type { get; set; } = null!;

        /// <summary>
        /// Статус
        /// </summary>
        public string Status { get; set; } = null!;

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; } = null!;

        /// <summary>
        /// Адрес жжлектронной почты
        /// </summary>
        public string Email { get; set; } = null!;
    }
}
