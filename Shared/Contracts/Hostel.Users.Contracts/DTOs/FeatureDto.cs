namespace Hostel.Users.Contracts
{
    public class FeatureDto
    {
        public string FeatureName { get; set; } = null!;
        public CrudType[] CrudTypes { get; set; } = null!;
    }
}
