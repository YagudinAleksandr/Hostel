using Hostel.Shared.Kernel;

namespace Hostel.SU.Domain
{
    /// <summary>
    /// Событие смены адреса жлектронной почты пользователя
    /// </summary>
    /// <param name="Id">Идентификатор пользователя</param>
    /// <param name="Email">Адрес жлектронной почты</param>
    public record UserChangedEmailEvent(Guid Id, string Email) : IDomainEvent
    {
        /// <inheritdoc/>
        public Guid EventId => Guid.NewGuid();

        /// <inheritdoc/>
        public DateTime OccuredOn => DateTime.UtcNow;
    }
}
