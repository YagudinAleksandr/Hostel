using System;

namespace Hostel.Infrastructure.Entities.Base
{
    public interface IBaseEntity
    {
        DateTimeOffset CreatedAt { get; set; }
        DateTimeOffset UpdatedAt { get; set; }
    }
}
