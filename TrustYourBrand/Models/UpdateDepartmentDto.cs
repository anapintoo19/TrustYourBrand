namespace TrustYourBrand.Models
{
    public class UpdateDepartmentDto
    {
        public string? Nome { get; set; }
        public bool? IsActive { get; set; }
        public List<int> UserIds { get; set; }
    }
}
