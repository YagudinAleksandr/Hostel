namespace Hostel.SU.Domain
{
    /// <summary>
    /// Тип пользователя "Пользователь"
    /// </summary>
    internal sealed record StandartType : UserType
    {
        public StandartType()
            : base(ServicesUsersTypeCodes.Standart, ServicesUsersTypeCodes.ServicesUsersUserTypeStandart)
        {

        }
    }
}
