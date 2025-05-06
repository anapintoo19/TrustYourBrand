namespace TrustYourBrand.Models
{
    public class CreateInspectionDto
    {
        public int LojaId { get; set; }
        public int UserId { get; set; }
        public DateTime DataInicio { get; set; }
        public bool IsActive { get; set; } = true;
        public string Descricao { get; set; }
        public string Estado { get; set; }
        public string Frequencia { get; set; }
        public DateTime DataFim { get; set; }
        public int? MarcaId { get; set; }
        public int? FormularioId { get; set; }
        public string Language { get; set; }
        public string InspecaoName { get; set; }
        public string Status { get; set; }
        public string Mode { get; set; } = "Launched";
        public bool IsCanceled { get; set; } = false;
        public List<int>? SeccaoIds { get; set; }
        public List<CreatePerguntaPersonalizadaDto>? PerguntasPersonalizadas { get; set; }
    }

    public class CreatePerguntaPersonalizadaDto
    {
        public string Texto { get; set; }
        public string TipoResposta { get; set; }
        public List<string>? Opcoes { get; set; }
        public string? Resposta { get; set; }
    }
}
