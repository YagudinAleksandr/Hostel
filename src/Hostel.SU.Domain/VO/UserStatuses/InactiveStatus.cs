namespace Hostel.SU.Domain
{
    /// <summary>
    /// Статус активности "Не активен"
    /// </summary>
    internal sealed record InactiveStatus : UserStatus
    {
        public InactiveStatus()
            : base(ServicesUsersStatusesCodes.ServicesUsersUserStatusInactiveCode, ServicesUsersStatusesCodes.ServicesUsersUserStatusInactive)
        {

        }
    }
}
