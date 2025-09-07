using Hostel.Shared.Kernel;

namespace Hostel.SU.Domain
{
    /// <summary>
    /// Исключение использованного токена
    /// </summary>
    public class DomainUsedTokenException : DomainException
    {
        /// <summary>
        /// Исключение использованного токена
        /// </summary>
        public DomainUsedTokenException()
            : base(ServicesUsersExceptionCodes.ServicesUsersExceptionUsedToken)
        {

        }
    }
}
