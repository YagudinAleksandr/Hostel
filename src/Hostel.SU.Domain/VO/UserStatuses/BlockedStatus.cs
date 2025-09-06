namespace Hostel.SU.Domain
{
    /// <summary>
    /// Статус активности "Заблокирован"
    /// </summary>
    internal sealed record BlockedStatus : UserStatus
    {
        public BlockedStatus()
            :base(ServicesUsersStatusesCodes.ServicesUsersUserStatusBlockedCode, ServicesUsersStatusesCodes.ServicesUsersUserStatusBlocked)
        {
            
        }
    }
}
