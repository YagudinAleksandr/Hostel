using Hostel.Infrastructure.Entities.Base;

namespace Hostel.Infrastructure.Entities
{
    public interface IUserEntity : IBaseEntity
    {
        string Fullname { get; set; }
        bool IsAdmin { get; set; }
        bool IsActive { get; set; }
        string ProfileImg { get; set; }
    }
}
