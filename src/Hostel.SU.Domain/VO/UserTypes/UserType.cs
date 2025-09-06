namespace Hostel.SU.Domain
{
    /// <summary>
    /// Тип пользователя
    /// </summary>
    public abstract record UserType
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
        /// Тип пользователя
        /// </summary>
        /// <param name="code">Код</param>
        /// <param name="displayName">Отображаемое имя</param>
        protected UserType(string code, string displayName)
        {
            Code = code;
            DisplayName = displayName;
        }
    }
}
