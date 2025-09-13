namespace Hostel.SU.Domain
{
    /// <summary>
    /// Реестр типов <see cref="UserType"/> пользователя
    /// </summary>
    public static class UserTypes
    {
        /// <summary>
        /// Пользователь
        /// </summary>
        public static readonly UserType Standart = new StandartType();

        /// <summary>
        /// Менеджер
        /// </summary>
        public static readonly UserType Manager = new ManagerType();

        /// <summary>
        /// Администратор
        /// </summary>
        public static readonly UserType Admin = new AdminType();

        /// <summary>
        /// Ситема
        /// </summary>
        public static readonly UserType System = new SystemType();

        /// <summary>
        /// Все
        /// </summary>
        public static IReadOnlyCollection<UserType> All { get; } = new[]
        {
            Standart,
            Manager,
            Admin,
            System
        };
    }
}
