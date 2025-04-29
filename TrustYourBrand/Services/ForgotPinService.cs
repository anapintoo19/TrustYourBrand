using System.Net.Http.Json;
using TrustYourBrand.Models;

namespace TrustYourBrand.Services
{
    public class ForgotPinService : IForgotPinService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ForgotPinService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ForgotPinResult> RequestVerificationCodeAsync(string phoneNumber)
        {
            var httpClient = _httpClientFactory.CreateClient("ApiClient");
            //var response = await httpClient.PostAsJsonAsync("www/api/auth/request-pin-reset", new { PhoneNumber = phoneNumber });
            var response = await httpClient.PostAsJsonAsync("api/auth/request-pin-reset", new { PhoneNumber = phoneNumber });
            if (response.IsSuccessStatusCode)
            {
                return new ForgotPinResult
                {
                    Success = true
                };
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Erro ao solicitar código de verificação: {response.StatusCode} - {errorContent}");
                return new ForgotPinResult
                {
                    Success = false,
                    ErrorMessage = $"Failed to request code: {response.StatusCode} - {errorContent}"
                };
            }
        }

        public async Task<ForgotPinResult> ResetPinAsync(string phoneNumber, string verificationCode, string newPin)
        {
            var httpClient = _httpClientFactory.CreateClient("ApiClient");
            //var response = await httpClient.PostAsJsonAsync("www/api/auth/reset-pin", new
            var response = await httpClient.PostAsJsonAsync("api/auth/reset-pin", new
            {
                PhoneNumber = phoneNumber,
                VerificationCode = verificationCode,
                NewPin = newPin
            });
            if (response.IsSuccessStatusCode)
            {
                return new ForgotPinResult
                {
                    Success = true
                };
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Erro ao redefinir PIN: {response.StatusCode} - {errorContent}");
                return new ForgotPinResult
                {
                    Success = false,
                    ErrorMessage = $"Failed to reset PIN: {response.StatusCode} - {errorContent}"
                };
            }
        }
    }
}