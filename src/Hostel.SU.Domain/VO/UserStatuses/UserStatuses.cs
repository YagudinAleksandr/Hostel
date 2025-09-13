namespace Hostel.SU.Domain
{
    /// <summary>
    /// Реестр статусов <see cref="UserStatus"/> пользователя
    /// </summary>
    public static class UserStatuses
    {
        // <summary>
        /// Статус "Активен"
        /// </summary>
        public static readonly UserStatus Active = new ActiveStatus();

        /// <summary>
        /// Статус "Не активен"
        /// </summary>
        public static readonly UserStatus Inactive = new InactiveStatus();

        /// <summary>
        /// Статус "Ожидает"
        /// </summary>
        public static readonly UserStatus Pending = new PendingStatus();

        /// <summary>
        /// Статус "Заблокирован"
        /// </summary>
        public static readonly UserStatus Blocked = new BlockedStatus();

        /// <summary>
        /// Все статусы
        /// </summary>
        public static IReadOnlyCollection<UserStatus> All { get; } = new[]
        {
            Active,
            Inactive,
            Pending,
            Blocked
        };
    }
}
