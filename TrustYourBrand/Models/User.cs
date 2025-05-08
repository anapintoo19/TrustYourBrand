using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TrustYourBrand.Models
{
    public class User
    {
        [JsonPropertyName("UserId")]
        public int Id { get; set; }
        [JsonPropertyName("TenantId")]
        public int TenantId { get; set; }
        [JsonPropertyName("RoleId")]
        public int? RoleId { get; set; }
        [JsonPropertyName("Role")]
        public string? Role { get; set; }
        [JsonPropertyName("PhoneNumber")]
        [RegularExpression(@"^[0-9]{9}$", ErrorMessage = "O número de telefone deve conter exatamente 9 dígitos numéricos.")]
        public string? PhoneNumber { get; set; }
        [JsonPropertyName("Pin")]
        public string? Pin { get; set; }
        [JsonPropertyName("FirstName")]
        public string? FirstName { get; set; }
        [JsonPropertyName("LastName")]
        public string? LastName { get; set; }
        [JsonPropertyName("Gender")]
        public string? Gender { get; set; }
        [JsonPropertyName("Country")]
        public string? Country { get; set; }
        [JsonPropertyName("Language")]
        public string? Language { get; set; }
        [JsonPropertyName("Tenant")]
        public string? Tenant { get; set; }
        [JsonPropertyName("MarcaId")]
        public int? MarcaId { get; set; }
        [JsonPropertyName("Marca")] // Map "Marca" to Brand
        public string? Brand { get; set; }
        [JsonPropertyName("Loja")] // Map "Loja" to Store
        public string? Store { get; set; }
        [JsonPropertyName("City")]
        public string? City { get; set; }
        [JsonPropertyName("Birthday")]
        public DateTime? Birthday { get; set; }
        [JsonPropertyName("Function")]
        public string? Function { get; set; }
        [JsonPropertyName("FunctionId")]
        public int? FunctionId { get; set; }
        [JsonPropertyName("DepartmentId")]
        public int? DepartmentId { get; set; }
        [JsonPropertyName("LojaId")]
        public int? LojaId { get; set; }
        [JsonPropertyName("Department")]
        public string? Department { get; set; }
        [JsonPropertyName("Email")]
        public string? Email { get; set; }
        [JsonPropertyName("IsActive")]
        public bool IsActive { get; set; }
        [JsonPropertyName("IsFirstLogin")]
        public bool IsFirstLogin { get; set; }
        [JsonPropertyName("CreatedByUserId")]
        public int CreatedByUserId { get; set; }
    }
}