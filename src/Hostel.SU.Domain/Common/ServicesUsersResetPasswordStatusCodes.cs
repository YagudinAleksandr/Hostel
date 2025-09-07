namespace Hostel.SU.Domain
{
    /// <summary>
    /// Статусы токенов для сброса паролей
    /// </summary>
    public static partial class ServicesUsersResetPasswordStatusCodes
    {
        /// <summary>
        /// Ожидает
        /// </summary>
        public const string ServicesUsersResetPasswordStatusPending = "services.users.reset_password_status.pending";

        /// <summary>
        /// Истек
        /// </summary>
        public const string ServicesUsersResetPasswordStatusExpired = "services.users.reset_password_status.expired";

        /// <summary>
        /// Использован
        /// </summary>
        public const string ServicesUsersResetPasswordStatusUsed = "services.users.reset_password_status.used";
    }
}
