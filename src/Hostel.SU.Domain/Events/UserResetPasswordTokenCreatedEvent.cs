using Hostel.Shared.Kernel;

namespace Hostel.SU.Domain
{
    /// <summary>
    /// Событие о генерации токена для сброса пароля
    /// </summary>
    /// <param name="UserId">Идентификатор пользователя</param>
    /// <param name="Token">Токен сброса</param>
    public record UserResetPasswordTokenCreatedEvent(Guid UserId, Guid Token) : IDomainEvent
    {
        /// <inheritdoc/>
        public Guid EventId => Guid.NewGuid();

        /// <inheritdoc/>
        public DateTime OccuredOn => DateTime.UtcNow;
    }
}
