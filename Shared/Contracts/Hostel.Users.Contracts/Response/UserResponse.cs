namespace Hostel.Users.Contracts.Response
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public record UserResponse
    {
        public Guid Id { get; set; }

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
        /// Адрес жжлектронной почты
        /// </summary>
        public string Email { get; set; } = null!;

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Дата изменения
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }
}
