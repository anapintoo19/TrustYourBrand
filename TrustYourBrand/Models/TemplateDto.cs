namespace TrustYourBrand.Models
{
    public class TemplateDto
    {
        public int TemplateId { get; set; }
        public string Nome { get; set; }
        public int TenantId { get; set; }
        public bool IsActive { get; set; }
    }
}
