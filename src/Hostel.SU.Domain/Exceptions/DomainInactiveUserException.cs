using Hostel.Shared.Kernel;

namespace Hostel.SU.Domain
{
    /// <summary>
    /// Исключение неактивного пользователя
    /// </summary>
    public class DomainInactiveUserException : DomainException
    {
        /// <summary>
        /// Исключение неактивного пользователя
        /// </summary>
        public DomainInactiveUserException() : base(ServicesUsersExceptionCodes.ServicesUsersExceptionInactiveUser)
        {
        }
    }
}
