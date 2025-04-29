namespace TrustYourBrand.Models
{
    public class SectionDto
    {
        public int SeccaoId { get; set; }
        public int TenantId { get; set; }
        public string Nome { get; set; } 
        public string Descricao { get; set; }
        public bool IsActive { get; set; } 
    }
}
