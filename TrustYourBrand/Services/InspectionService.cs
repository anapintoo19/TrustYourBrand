using Blazored.LocalStorage;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using TrustYourBrand.Models;

namespace TrustYourBrand.Services
{
    public class InspectionService : IInspectionService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;
        public InspectionService(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }
    
        public async Task<List<InspectionDto>> GetInspectionsAsync()
        {
            try
            {
                var token = await _localStorageService.GetItemAsync<string>("authToken");
                if (string.IsNullOrEmpty(token))
                {
                    Console.WriteLine("Token de autenticação não encontrado.");
                    return new List<InspectionDto>();
                }

                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync("www/api/Inspecao");
                var json = await response.Content.ReadAsStringAsync();

                Console.WriteLine("✅ Conteúdo da resposta da API:");
                Console.WriteLine(json);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"❌ Erro ao obter inspeções: {response.StatusCode} - {json}");
                    return new List<InspectionDto>();
                }

                var inspections = JsonSerializer.Deserialize<List<InspectionDto>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                Console.WriteLine($"✅ {inspections?.Count ?? 0} inspeções carregadas da API.");
                return inspections ?? new List<InspectionDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao obter inspeções: {ex.Message}");
                return new List<InspectionDto>();
            }
        }

        public async Task<InspectionDto> GetInspectionByIdAsync(int id)
        {
            try
            {
                var token = await _localStorageService.GetItemAsync<string>("authToken");
                if (string.IsNullOrEmpty(token))
                {
                    Console.WriteLine("Token de autenticação não encontrado.");
                    throw new Exception("Authentication token not found.");
                }

                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync($"www/api/Inspecao/{id}");
                var json = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"✅ Resposta da API para inspeção {id}:");
                Console.WriteLine(json);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"❌ Erro ao obter inspeção {id}: {response.StatusCode} - {json}");
                    throw new Exception($"Error fetching inspection: {response.StatusCode}");
                }

                var inspection = JsonSerializer.Deserialize<InspectionDto>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return inspection ?? throw new Exception("Inspection not found.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao obter inspeção: {ex.Message}");
                throw new Exception($"Error fetching inspection: {ex.Message}");
            }
        }

        public async Task<InspectionResult> CreateInspectionAsync(CreateInspectionDto inspectionDto)
        {
            try
            {
                if (inspectionDto == null)
                {
                    return new InspectionResult
                    {
                        Success = false,
                        ErrorMessage = "The inspection object is null."
                    };
                }

                var token = await _localStorageService.GetItemAsync<string>("authToken");
                if (string.IsNullOrEmpty(token))
                {
                    return new InspectionResult
                    {
                        Success = false,
                        ErrorMessage = "Authentication token not found."
                    };
                }

                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                Console.WriteLine($"Enviando requisição POST para api/Inspecao: {JsonSerializer.Serialize(inspectionDto)}");
                var response = await _httpClient.PostAsJsonAsync("www/api/Inspecao", inspectionDto);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Resposta do POST: Status {response.StatusCode}, Conteúdo: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<InspectionCreationResponse>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (result == null || result.InspecaoId == 0)
                    {
                        return new InspectionResult
                        {
                            Success = false,
                            ErrorMessage = "API returned an invalid or null result."
                        };
                    }

                    return new InspectionResult
                    {
                        Success = true,
                        Message = "Inspection created successfully.",
                        CreatedInspectionId = result.InspecaoId
                    };
                }
                else
                {
                    return new InspectionResult
                    {
                        Success = false,
                        ErrorMessage = $"Error creating inspection: {response.StatusCode} - {responseContent}"
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Exceção no CreateInspection: {ex.GetType().Name} - {ex.Message}");
                return new InspectionResult
                {
                    Success = false,
                    ErrorMessage = $"Error when making the request: {ex.Message}"
                };
            }
        }

        public async Task<InspectionResult> UpdateInspectionAsync(int id, UpdateInspectionDto inspectionDto)
        {
            try
            {
                var token = await _localStorageService.GetItemAsync<string>("authToken");
                if (string.IsNullOrEmpty(token))
                {
                    return new InspectionResult
                    {
                        Success = false,
                        ErrorMessage = "Authentication token not found."
                    };
                }

                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                Console.WriteLine($"Enviando requisição PUT para api/Inspecao/{id}: {JsonSerializer.Serialize(inspectionDto)}");
                var response = await _httpClient.PutAsJsonAsync($"www/api/Inspecao/{id}", inspectionDto);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Resposta do PUT: Status {response.StatusCode}, Conteúdo: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    return new InspectionResult
                    {
                        Success = true,
                        Message = "Inspection updated successfully."
                    };
                }
                else
                {
                    return new InspectionResult
                    {
                        Success = false,
                        ErrorMessage = $"Error updating inspection: {response.StatusCode} - {responseContent}"
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Exceção no UpdateInspection: {ex.GetType().Name} - {ex.Message}");
                return new InspectionResult
                {
                    Success = false,
                    ErrorMessage = $"Error when making the request: {ex.Message}"
                };
            }
        }

        public async Task<InspectionResult> DeleteInspectionAsync(int id)
        {
            try
            {
                var token = await _localStorageService.GetItemAsync<string>("authToken");
                if (string.IsNullOrEmpty(token))
                {
                    return new InspectionResult
                    {
                        Success = false,
                        ErrorMessage = "Authentication token not found."
                    };
                }

                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                Console.WriteLine($"Fazendo requisição DELETE para api/Inspecao/{id}");
                var response = await _httpClient.DeleteAsync($"www/api/Inspecao/{id}");
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Resposta do DELETE: Status {response.StatusCode}, Conteúdo: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    return new InspectionResult
                    {
                        Success = true,
                        Message = "Inspection successfully deactivated."
                    };
                }
                else
                {
                    return new InspectionResult
                    {
                        Success = false,
                        ErrorMessage = $"Error deactivating inspection: {response.StatusCode} - {responseContent}"
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Exceção no DeleteInspection: {ex.GetType().Name} - {ex.Message}");
                return new InspectionResult
                {
                    Success = false,
                    ErrorMessage = $"Error when making the request: {ex.Message}"
                };
            }
        }
    }
    public class InspectionResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? ErrorMessage { get; set; }
        public int? CreatedInspectionId { get; set; }
    }

    public class InspectionCreationResponse
    {
        public int InspecaoId { get; set; }
        public bool PermitePerguntasPersonalizadas { get; set; }
    }
}
