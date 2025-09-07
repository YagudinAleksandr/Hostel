namespace Hostel.SU.Domain
{
    /// <summary>
    /// Статус токена для сброса пароля
    /// </summary>
    public abstract record ResetPasswordStatus
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
        /// Статус токена для сброса пароля
        /// </summary>
        /// <param name="code">Код</param>
        /// <param name="displayName">Отображаемое имя</param>
        protected ResetPasswordStatus(string code, string displayName)
        {
            Code = code;
            DisplayName = displayName;
        }
    }
}
