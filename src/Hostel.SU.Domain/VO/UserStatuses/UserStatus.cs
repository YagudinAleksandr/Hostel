namespace Hostel.SU.Domain
{
    /// <summary>
    /// Статус пользователя
    /// </summary>
    public abstract record UserStatus
    {
        /// <summary>
        /// Код
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Отображаемое имя
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// Статус пользователя
        /// </summary>
        /// <param name="code">Код</param>
        /// <param name="displayName">Отображаемое имя</param>
        protected UserStatus(string code, string displayName)
        {
            Code = code;
            DisplayName = displayName;
        }
    }
}
