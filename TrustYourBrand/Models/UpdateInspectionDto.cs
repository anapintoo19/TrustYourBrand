namespace TrustYourBrand.Models
{
    public class UpdateInspectionDto
    {
        public int Id { get; set; }
        public List<int>? LojaIds { get; set; }
        public int? UserId { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public string? Descricao { get; set; }
        public string? Frequencia { get; set; }
        public string? InspecaoName { get; set; }
        public string? Estado { get; set; }
        public bool IsActive { get; set; }
        public int? FormularioId { get; set; }
        public int? MarcaId { get; set; }
        public string? Language { get; set; }
        public string? Status { get; set; }
        public string? Mode { get; set; }
        public List<int>? SeccaoIds { get; set; }
        public List<CreatePerguntaPersonalizadaDto>? PerguntasPersonalizadas { get; set; }
    }
}
