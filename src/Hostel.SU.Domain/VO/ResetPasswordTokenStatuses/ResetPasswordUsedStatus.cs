namespace Hostel.SU.Domain
{
    /// <summary>
    /// Статус токена сброса пароял "Использован"
    /// </summary>
    internal sealed record ResetPasswordUsedStatus : ResetPasswordStatus
    {
        public ResetPasswordUsedStatus()
            : base(ServicesUsersResetPasswordStatusCodes.Used, ServicesUsersResetPasswordStatusCodes.ServicesUsersResetPasswordStatusUsed)
        {

        }
    }
}
