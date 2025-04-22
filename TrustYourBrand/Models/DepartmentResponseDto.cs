namespace TrustYourBrand.Models
{
    public class DepartmentResponseDto
    {
        public int DepartmentId { get; set; }
        public int TenantId { get; set; }
        public string Nome { get; set; }
        public bool IsActive { get; set; }
        public List<int> UserIds { get; set; }
    }
}
