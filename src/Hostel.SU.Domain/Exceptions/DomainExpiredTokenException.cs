using Hostel.Shared.Kernel;

namespace Hostel.SU.Domain
{
    /// <summary>
    /// Исключение истекшего токена
    /// </summary>
    public class DomainExpiredTokenException : DomainException
    {
        /// <summary>
        /// Исключение истекшего токена
        /// </summary>
        public DomainExpiredTokenException()
            :base(ServicesUsersExceptionCodes.ServicesUsersExceptionExpiredToken)
        {
            
        }
    }
}
