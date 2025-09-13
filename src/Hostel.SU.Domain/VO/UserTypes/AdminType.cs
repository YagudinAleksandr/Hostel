namespace Hostel.SU.Domain
{
    /// <summary>
    /// Тип пользователя "Администратор"
    /// </summary>
    internal sealed record AdminType : UserType
    {
        public AdminType()
            : base(ServicesUsersTypeCodes.Admin, ServicesUsersTypeCodes.ServicesUsersUserTypeAdmin)
        {

        }
    }
}
