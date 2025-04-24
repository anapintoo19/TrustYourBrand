using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using TrustYourBrand.Models;

namespace TrustYourBrand.Services
{
    public class LoginService : ILoginService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILocalStorageService _localStorage;

        public LoginService(IHttpClientFactory httpClientFactory, ILocalStorageService localStorage)
        {
            _httpClientFactory = httpClientFactory;
            _localStorage = localStorage;
        }

        public async Task<LoginResult> LoginAsync(LoginModel model)
        {
            var httpClient = _httpClientFactory.CreateClient("ApiClient");
            var response = await httpClient.PostAsJsonAsync("www/api/auth/login", model);
            if (response.IsSuccessStatusCode)
            {
                var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>(new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                if (tokenResponse != null && !string.IsNullOrEmpty(tokenResponse.Token))
                {
                    await _localStorage.SetItemAsync("authToken", tokenResponse.Token);
                    return new LoginResult
                    {
                        Success = true,
                        Token = tokenResponse.Token,
                        IsFirstLogin = tokenResponse.IsFirstLogin,
                        Message = tokenResponse.Message ?? "Successful login."
                    };
                }
                return new LoginResult
                {
                    Success = false,
                    ErrorMessage = "Token not found in response."
                };
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Erro ao fazer login: {response.StatusCode} - {errorContent}");
                return new LoginResult
                {
                    Success = false,
                    ErrorMessage = $"Login failed: {response.StatusCode} - {errorContent}"
                };
            }
        }

        public async Task<bool> CheckUserExists(string phoneNumber)
        {
            var httpClient = _httpClientFactory.CreateClient("ApiClient");
            var response = await httpClient.GetAsync($"www/api/auth/checkuserexists/{phoneNumber}");
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Erro ao verificar usuário: {response.StatusCode} - {errorContent}");
            }
            return response.IsSuccessStatusCode;
        }

        public async Task<ChangePinResult> ChangePinAsync(int userId, string newPin, string confirmNewPin)
        {
            var httpClient = _httpClientFactory.CreateClient("ApiClient");

            // Obter o token do local storage para autenticação
            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (string.IsNullOrEmpty(token))
            {
                return new ChangePinResult
                {
                    Success = false,
                    ErrorMessage = "Authentication token not found. Log in again."
                };
            }

            // Adicionar o token ao cabeçalho da requisição
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            // Enviar NewPin e ConfirmNewPin no corpo da requisição
            var setPinDto = new { NewPin = newPin, ConfirmNewPin = confirmNewPin };
            var response = await httpClient.PostAsJsonAsync("www/api/auth/set-pin", setPinDto); // Ajustado para o endpoint correto
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ChangePinResponse>(new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return new ChangePinResult
                {
                    Success = true,
                    ErrorMessage = result?.Message
                };
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Erro ao definir PIN: {response.StatusCode} - {errorContent}");
                return new ChangePinResult
                {
                    Success = false,
                    ErrorMessage = $"Failed to set PIN: {response.StatusCode} - {errorContent}"
                };
            }
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
        }
    }

    public class TokenResponse
    {
        public string? Token { get; set; }
        public bool IsFirstLogin { get; set; }
        public string? Message { get; set; }
    }

    public class ChangePinResponse
    {
        public string? Message { get; set; }
    }
}