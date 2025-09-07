namespace Hostel.SU.Domain
{
    /// <summary>
    /// Статус токена сброса пароля "Ожидает"
    /// </summary>
    internal sealed record ResetPasswordPendingStatus : ResetPasswordStatus
    {
        public ResetPasswordPendingStatus()
            : base(ServicesUsersResetPasswordStatusCodes.Pending, ServicesUsersResetPasswordStatusCodes.ServicesUsersResetPasswordStatusPending)
        {
            
        }
    }
}
