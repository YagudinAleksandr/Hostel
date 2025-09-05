using Hostel.Shared.Kernel;

namespace Hostel.SH.Domain
{
    public class Hostel : Entity<int>
    {
        public string Name { get; private set; }
        public AddressVo Address { get; private set; }
    }
}
