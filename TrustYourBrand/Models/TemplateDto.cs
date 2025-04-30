using System.Text.Json.Serialization;

namespace TrustYourBrand.Models
{
    public class TemplateDto
    {
        public int FormularioId { get; set; }
        public string Nome { get; set; }
        public int TenantId { get; set; }
        public bool IsActive { get; set; }
    }
}
