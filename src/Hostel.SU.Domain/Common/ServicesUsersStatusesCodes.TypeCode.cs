namespace Hostel.SU.Domain
{
    /// <summary>
    /// Коды статусов
    /// </summary>
    public static partial class ServicesUsersStatusesCodes
    {
        /// <summary>
        /// Активен
        /// </summary>
        public const string ServicesUsersUserStatusActiveCode = "ACTIVE";

        /// <summary>
        /// Ожидает
        /// </summary>
        public const string ServicesUsersUserStatusPendingCode = "PENDING";

        /// <summary>
        /// Не активен
        /// </summary>
        public const string ServicesUsersUserStatusInactiveCode = "INACTIVE";

        /// <summary>
        /// Заблокирован
        /// </summary>
        public const string ServicesUsersUserStatusBlockedCode = "BLOCKED";
    }
}
