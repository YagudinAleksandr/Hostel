using Hostel.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using System;

namespace Hostel.DAL.Entities
{
    public class UserEntity: IdentityUser, IUserEntity
    {
        public string Fullname { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
        public string ProfileImg { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
