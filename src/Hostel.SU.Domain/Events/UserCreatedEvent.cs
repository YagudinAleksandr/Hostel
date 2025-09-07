
using Hostel.Shared.Kernel;

namespace Hostel.SU.Domain
{
    /// <summary>
    /// Событие создания пользователя
    /// </summary>
    /// <param name="Id">Идентификатор</param>
    /// <param name="Email">Адрес электронной почты</param>
    public record UserCreatedEvent(Guid Id, string Email) : IDomainEvent
    {
        /// <inheritdoc/>
        public Guid EventId => Guid.NewGuid();

        /// <inheritdoc/>
        public DateTime OccuredOn => DateTime.UtcNow;
    }
}
