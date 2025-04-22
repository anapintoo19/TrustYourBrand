namespace TrustYourBrand.Models
{
    public class LoginModel
    {
        public string? PhoneNumber { get; set; }
        public string? Pin { get; set; }
    }

    public class LoginResult
    {
        public bool Success { get; set; }
        public string? Token { get; set; }
        public string? ErrorMessage { get; set; }
        public bool IsFirstLogin { get; set; }
        public string? CurrentPin { get; set; }
        public string? Message { get; set; }
        public User? CreatedUser { get; set; }
    }

    public class ChangePinResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
    }
}