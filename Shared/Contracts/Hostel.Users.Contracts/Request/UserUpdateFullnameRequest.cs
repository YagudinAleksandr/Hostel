namespace Hostel.Users.Contracts.Request
{
    /// <summary>
    /// Запрос на изменение ФИО пользователя
    /// </summary>
    public record UserUpdateFullnameRequest
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string Lastname { get; set; } = null!;

        /// <summary>
        /// Имя
        /// </summary>
        public string Firstname { get; set; } = null!;

        /// <summary>
        /// Отчество
        /// </summary>
        public string? Patronymic { get; set; }
    }
}
