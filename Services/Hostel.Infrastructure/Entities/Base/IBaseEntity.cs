using System;

namespace Hostel.Infrastructure.Entities.Base
{
    /// <summary>
    /// Интерфейс базовой сущности
    /// </summary>
    public interface IBaseEntity
    {
        /// <summary>
        /// Дата создания
        /// </summary>
        DateTimeOffset CreatedAt { get; set; }
        /// <summary>
        /// Дата изменения
        /// </summary>
        DateTimeOffset UpdatedAt { get; set; }
    }
}
