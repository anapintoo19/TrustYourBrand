namespace TrustYourBrand.Services
{
    public interface IForgotPinService
    {
        Task<ForgotPinResult> RequestVerificationCodeAsync(string phoneNumber);
        Task<ForgotPinResult> ResetPinAsync(string phoneNumber, string verificationCode, string newPin);
    }

    public class ForgotPinResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
    }
}