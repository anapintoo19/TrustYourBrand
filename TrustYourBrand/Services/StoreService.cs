using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using TrustYourBrand.Models;
using TrustYourBrand.Pages;

namespace TrustYourBrand.Services
{
    public class StoreService : IStoreService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;

        public StoreService(IHttpClientFactory httpClientFactory, ILocalStorageService localStorageService)
        {
            _httpClient = httpClientFactory.CreateClient("StoreApiClient");
            _localStorageService = localStorageService;
        }

        public async Task<List<StoreDto>> GetStoresAsync()
        {
            try
            {
                var token = await _localStorageService.GetItemAsync<string>("authToken");
                if (string.IsNullOrEmpty(token))
                {
                    Console.WriteLine("Token de autenticação não encontrado.");
                    return new List<StoreDto>();
                }

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.GetAsync("www/api/Store");
                var json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Erro ao obter lojas: {response.StatusCode} - {json}");
                    return new List<StoreDto>();
                }

                return JsonSerializer.Deserialize<List<StoreDto>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<StoreDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter lojas: {ex.Message}");
                return new List<StoreDto>();
            }
        }

        public async Task<StoreResult> CreateStoreAsync(CreateStoreDto storeDto)
        {
            try
            {
                var token = await _localStorageService.GetItemAsync<string>("authToken");
                if (string.IsNullOrEmpty(token))
                {
                    return new StoreResult
                    {
                        Success = false,
                        ErrorMessage = "Token de autenticação não encontrado."
                    };
                }

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PostAsJsonAsync("www/api/Store", storeDto);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Erro ao criar loja: {response.StatusCode} - {responseContent}");
                    return new StoreResult
                    {
                        Success = false,
                        ErrorMessage = $"Erro ao criar loja: {responseContent}"
                    };
                }

                var result = JsonSerializer.Deserialize<CreateStoreResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var createdStore = new StoreDto
                {
                    LojaId = result.LojaId,
                    TenantId = storeDto.TenantId,
                    MarcaId = storeDto.MarcaId,
                    Nome = storeDto.Nome,
                    Zona = storeDto.Zona,
                    IsActive = true,
                    Email = storeDto.Email,
                    PhoneNumber = storeDto.PhoneNumber,
                    Address = storeDto.Address,
                    Country = storeDto.Country,
                    City = storeDto.City,
                    Condition = storeDto.Condition,
                    Inspector = storeDto.Inspector,
                    StoreManager = storeDto.StoreManager,
                    StoreType = storeDto.StoreType
                };

                return new StoreResult
                {
                    Success = true,
                    Message = "Loja criada com sucesso.",
                    CreatedStore = createdStore
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao criar loja: {ex.Message}");
                return new StoreResult
                {
                    Success = false,
                    ErrorMessage = $"Erro ao fazer a requisição: {ex.Message}"
                };
            }
        }

        public async Task<StoreResult> UpdateStoreAsync(int id, UpdateStoreDto storeDto)
        {
            try
            {
                var token = await _localStorageService.GetItemAsync<string>("authToken");
                if (string.IsNullOrEmpty(token))
                {
                    return new StoreResult
                    {
                        Success = false,
                        ErrorMessage = "Token de autenticação não encontrado."
                    };
                }

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PutAsJsonAsync($"www/api/Store/{id}", storeDto);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Erro ao atualizar loja: {response.StatusCode} - {responseContent}");
                    return new StoreResult
                    {
                        Success = false,
                        ErrorMessage = $"Erro ao atualizar loja: {responseContent}"
                    };
                }

                var updatedStore = JsonSerializer.Deserialize<StoreDto>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return new StoreResult
                {
                    Success = true,
                    Message = "Loja atualizada com sucesso.",
                    UpdatedStore = updatedStore
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar loja: {ex.Message}");
                return new StoreResult
                {
                    Success = false,
                    ErrorMessage = $"Erro ao fazer a requisição: {ex.Message}"
                };
            }
        }

        public async Task<StoreResult> DeleteStoreAsync(int id)
        {
            try
            {
                var token = await _localStorageService.GetItemAsync<string>("authToken");
                if (string.IsNullOrEmpty(token))
                {
                    return new StoreResult
                    {
                        Success = false,
                        ErrorMessage = "Token de autenticação não encontrado."
                    };
                }

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.DeleteAsync($"www/api/Store/{id}");
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Erro ao excluir loja: {response.StatusCode} - {responseContent}");
                    return new StoreResult
                    {
                        Success = false,
                        ErrorMessage = $"Erro ao excluir loja: {responseContent}"
                    };
                }

                return new StoreResult
                {
                    Success = true,
                    Message = "Loja excluída com sucesso."
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao excluir loja: {ex.Message}");
                return new StoreResult
                {
                    Success = false,
                    ErrorMessage = $"Erro ao fazer a requisição: {ex.Message}"
                };
            }
        }
    }

    // Classe auxiliar para deserializar a resposta da API
    public class CreateStoreResponse
    {
        public int LojaId { get; set; }
    }
}
