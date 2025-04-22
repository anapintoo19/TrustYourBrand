namespace TrustYourBrand.Models
{
    public class UpdateInspectionDto
    {
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public string? Descricao { get; set; }
        public string? Estado { get; set; }
        public string? Frequencia { get; set; }
        public bool? IsActive { get; set; }
        public int? MarcaId { get; set; }
        public int? FormularioId { get; set; }
        public string? InspecaoName { get; set; }
        public string? Status { get; set; }
        public List<int>? SeccaoIds { get; set; }
    }
}
