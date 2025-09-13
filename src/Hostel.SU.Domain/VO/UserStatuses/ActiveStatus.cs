namespace Hostel.SU.Domain
{
    /// <summary>
    /// Статус активности "Активен"
    /// </summary>
    internal sealed record ActiveStatus : UserStatus
    {
        public ActiveStatus() 
            : base(ServicesUsersStatusesCodes.ServicesUsersUserStatusActiveCode, ServicesUsersStatusesCodes.ServicesUsersUserStatusActive) { }
    }
}
