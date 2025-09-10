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
        public string Lastname { get; set; } = null!;
        public string Firstname { get; set; } = null!;
        public string? Patronymic { get; set; }
    }
}
