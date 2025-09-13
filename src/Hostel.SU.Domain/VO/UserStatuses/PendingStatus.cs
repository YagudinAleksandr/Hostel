namespace Hostel.SU.Domain
{
    /// <summary>
    /// Статус активности "Ожидает"
    /// </summary>
    internal sealed record PendingStatus : UserStatus
    {
        public PendingStatus()
            : base(ServicesUsersStatusesCodes.ServicesUsersUserStatusPendingCode, ServicesUsersStatusesCodes.ServicesUsersUserStatusPending)
        {

        }
    }
}
