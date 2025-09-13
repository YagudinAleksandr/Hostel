using Hostel.Shared.Kernel;

namespace Hostel.SU.Domain
{
    /// <summary>
    /// Событие изменения статуса пользователя
    /// </summary>
    /// <param name="Id">Идентификатор пользователя</param>
    /// <param name="StatusName">Статус</param>
    public record UserChangedStatusEvent(Guid Id, string StatusName) : IDomainEvent
    {
        /// <inheritdoc/>
        public Guid EventId => Guid.NewGuid();

        /// <inheritdoc/>
        public DateTime OccuredOn => DateTime.UtcNow;
    }
}
