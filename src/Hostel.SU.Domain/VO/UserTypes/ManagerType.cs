namespace Hostel.SU.Domain
{
    /// <summary>
    /// Тип пользователя "Менеджер"
    /// </summary>
    internal sealed record ManagerType : UserType
    {
        public ManagerType()
            : base(ServicesUsersTypeCodes.Manager, ServicesUsersTypeCodes.ServicesUsersUserTypeManager)
        {

        }
    }
}
