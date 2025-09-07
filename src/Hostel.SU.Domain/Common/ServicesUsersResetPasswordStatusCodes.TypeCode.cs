namespace Hostel.SU.Domain
{
    public static partial class ServicesUsersResetPasswordStatusCodes
    {
        /// <summary>
        /// Ожидает
        /// </summary>
        public const string Pending = "PENDING";

        /// <summary>
        /// Истек
        /// </summary>
        public const string Expired = "EXPIRED";

        /// <summary>
        /// Использован
        /// </summary>
        public const string Used = "Used";
    }
}
