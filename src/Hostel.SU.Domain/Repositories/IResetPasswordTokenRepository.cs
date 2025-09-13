using Hostel.Shared.Kernel;

namespace Hostel.SU.Domain
{
    /// <summary>
    /// Репозиторий для токенов сброса пароля
    /// </summary>
    public interface IResetPasswordTokenRepository : IRepository<UserResetPasswordToken, Guid>
    {
    }
}
