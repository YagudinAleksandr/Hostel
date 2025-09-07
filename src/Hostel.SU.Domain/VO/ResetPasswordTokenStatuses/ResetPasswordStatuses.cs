namespace Hostel.SU.Domain
{
    /// <summary>
    /// Реестр статусов токенов сброса пароля
    /// </summary>
    public static class ResetPasswordStatuses
    {
        /// <summary>
        /// Ожидает
        /// </summary>
        public static readonly ResetPasswordStatus Pending = new ResetPasswordPendingStatus();

        /// <summary>
        /// Истёк
        /// </summary>
        public static readonly ResetPasswordStatus Expired = new ResetPasswordExpiredStatus();

        /// <summary>
        /// Использован
        /// </summary>
        public static readonly ResetPasswordStatus Used = new ResetPasswordUsedStatus();

        /// <summary>
        /// Все
        /// </summary>
        public static IReadOnlyCollection<ResetPasswordStatus> All { get; } = new[]
        {
            Pending,
            Expired,
            Used
        };
    }
}
