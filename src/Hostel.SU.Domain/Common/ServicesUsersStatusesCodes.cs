namespace Hostel.SU.Domain
{
    /// <summary>
    /// Статусы пользователя
    /// </summary>
    public static partial class ServicesUsersStatusesCodes
    {
        /// <summary>
        /// Активен
        /// </summary>
        public const string ServicesUsersUserStatusActive = "services.users.user_status.active";

        /// <summary>
        /// Ожидает
        /// </summary>
        public const string ServicesUsersUserStatusPending = "services.users.user_status.pending";

        /// <summary>
        /// Не активен
        /// </summary>
        public const string ServicesUsersUserStatusInactive = "services.users.user_status.inactive";

        /// <summary>
        /// Заблокирован
        /// </summary>
        public const string ServicesUsersUserStatusBlocked = "services.users.user_status.blocked";
    }
}
