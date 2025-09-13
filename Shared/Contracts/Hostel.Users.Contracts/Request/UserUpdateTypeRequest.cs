namespace Hostel.Users.Contracts.Request
{
    /// <summary>
    /// Запрос на обновление типа пользователя
    /// </summary>
    public record UserUpdateTypeRequest
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Код типа
        /// </summary>
        public string TypeCode { get; set; } = null!;
    }
}
