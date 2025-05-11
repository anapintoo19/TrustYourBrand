namespace TrustYourBrand.Models
{
    public class InspectionDto
    {
        public int Id { get; set; }
        public int? TemplateId { get; set; }
        public string? Type { get; set; }
        public string Mode { get; set; } = "Launched";
        public string? Language { get; set; }
        public StatusBreakdown? StatusBreakdown { get; set; }
        public DateTime? FinishDate { get; set; }
        public string? Purpose { get; set; }
        public int? StoresCount { get; set; }
        public string? InspectionName { get; set; }
        public string? Frequency { get; set; }
        public string? Status { get; set; }
        public string? Estado { get; set; }
        public bool IsCanceled { get; set; } = false; 
        public bool IsActive { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string? Description { get; set; }
        public string? Brand { get; set; }
        public int? MarcaId { get; set; }
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }

        public List<int> LojaIds { get; set; }
        public int? UserId { get; set; }
        public List<int>? SeccaoIds { get; set; }
        public double ProgressPercentage => TotalTasks > 0 ? (CompletedTasks * 100.0) / TotalTasks : 0;
        public List<int> SelectedStoreIds { get; set; } = new List<int>();
        public List<int> SelectedGuestIds { get; set; } = new List<int>();
        public List<CustomQuestion> Questions { get; set; } = new List<CustomQuestion>();
    }

    // Classe para perguntas personalizadas
    public class CustomQuestion
    {
        public int Id { get; set; }
        public int? SeccaoId { get; set; }
        public int? TemplateId { get; set; }
        public int? InspecaoId { get; set; }
        public string Text { get; set; }
        public string Resposta { get; set; }
        public string ResponseType { get; set; }
        public List<string>? Options { get; set; }
        public bool IsDropdownEnabled { get; set; }
    }

    public class StatusBreakdown
    {
        public int Ongoing { get; set; }
        public int Closed { get; set; }
        public int Standby { get; set; }
        public int Canceled { get; set; }
        public int Total { get; set; }
        //public int Total => Ongoing + Closed + Standby + Canceled;
    }
}
