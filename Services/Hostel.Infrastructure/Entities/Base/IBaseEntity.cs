using System;

namespace Hostel.Infrastructure.Entities.Base
{
    public interface IBaseEntity<T>
    {
        T ID { get; set; }
        DateTimeOffset CreatedAt { get; set; }
        DateTimeOffset UpdatedAt { get; set; }
    }
}
