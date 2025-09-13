namespace Hostel.SU.Domain
{
    /// <summary>
    /// Статус токена сброса пароля "Истёк"
    /// </summary>
    internal sealed record ResetPasswordExpiredStatus : ResetPasswordStatus
    {
        public ResetPasswordExpiredStatus()
            : base(ServicesUsersResetPasswordStatusCodes.Expired, ServicesUsersResetPasswordStatusCodes.ServicesUsersResetPasswordStatusExpired)
        {

        }
    }
}
