namespace Hostel.Shared.Kernel
{
    /// <summary>
    /// Базовое доменное событие
    /// </summary>
    public interface IDomainEvent
    {
        /// <summary>
        /// Идентификатор события
        /// </summary>
        Guid EventId { get; }

        /// <summary>
        /// Дата создания события
        /// </summary>
        DateTime OccuredOn { get; }
    }
}
