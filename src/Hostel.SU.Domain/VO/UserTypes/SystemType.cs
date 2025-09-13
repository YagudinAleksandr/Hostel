namespace Hostel.SU.Domain
{
    /// <summary>
    /// Тип пользователя "Система"
    /// </summary>
    internal sealed record SystemType : UserType
    {
        public SystemType()
            : base(ServicesUsersTypeCodes.System, ServicesUsersTypeCodes.ServicesUsersUserTypeSystem)
        {

        }
    }
}
