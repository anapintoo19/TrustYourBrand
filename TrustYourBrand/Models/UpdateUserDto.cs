namespace TrustYourBrand.Models
{
    public class UpdateUserDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public int? DepartmentId { get; set; }
        public DateTime? Birthday { get; set; }
        public string? Gender { get; set; }
        public string? Position { get; set; }
        public int? RoleId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsFirstLogin { get; set; }
        public string? Pin { get; set; }
        public int? FunctionId { get; set; }
    }
}
