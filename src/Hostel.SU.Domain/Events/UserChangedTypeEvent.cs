using Hostel.Shared.Kernel;

namespace Hostel.SU.Domain
{
    /// <summary>
    /// Событие смены типа пользователя
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <param name="TypeName">Тип</param>
    public record UserChangedTypeEvent(Guid id, string TypeName) : IDomainEvent
    {
        /// <inheritdoc/>
        public Guid EventId => Guid.NewGuid();

        /// <inheritdoc/>
        public DateTime OccuredOn => DateTime.UtcNow;
    }
}
