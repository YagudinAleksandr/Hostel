namespace Hostel.SU.Domain
{
    /// <summary>
    /// Коды исключений сервиса
    /// </summary>
    public static class ServicesUsersExceptionCodes
    {
        /// <summary>
        /// Неактивный пользователь
        /// </summary>
        public const string ServicesUsersExceptionInactiveUser = "services.users.exception.inactive_user";

        /// <summary>
        /// Истекший токен
        /// </summary>
        public const string ServicesUsersExceptionExpiredToken = "services.users.exception.expired_token";

        /// <summary>
        /// Использованный токен
        /// </summary>
        public const string ServicesUsersExceptionUsedToken = "services.users.exception.used_token";
    }
}
