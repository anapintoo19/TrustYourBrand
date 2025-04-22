using System.Text.Json.Serialization;

namespace TrustYourBrand.Models
{
    public class User
    {
        [JsonPropertyName("UserId")]
        public int Id { get; set; }
        public int TenantId { get; set; }
        public int? RoleId { get; set; }
        public string? Role { get; set; } // Novo campo para o nome da role
        public string? PhoneNumber { get; set; }
        public string? Pin { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public string? Function { get; set; }
        public int? FunctionId { get; set; }
        public int? DepartmentId { get; set; }
        public string? Department { get; set; } // Novo campo para o nome do departamento
        public string? Email { get; set; }
        public bool IsActive { get; set; }
        public bool IsFirstLogin { get; set; }
    }
}