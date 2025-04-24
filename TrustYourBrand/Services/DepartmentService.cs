using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using TrustYourBrand.Models;
using TrustYourBrand.Pages;

namespace TrustYourBrand.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;

        public DepartmentService(IHttpClientFactory httpClientFactory, ILocalStorageService localStorageService)
        {
            _httpClient = httpClientFactory.CreateClient("DepartmentApiClient");
            _localStorageService = localStorageService;
        }

        public async Task<List<DepartmentResponseDto>> GetDepartmentsAsync()
        {
            try
            {
                var token = await _localStorageService.GetItemAsync<string>("authToken");
                if (string.IsNullOrEmpty(token))
                {
                    Console.WriteLine("Token de autenticação não encontrado.");
                    return new List<DepartmentResponseDto>();
                }

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.GetAsync("www/api/department");
                var json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Erro ao obter departamentos: {response.StatusCode} - {json}");
                    return new List<DepartmentResponseDto>();
                }

                return JsonSerializer.Deserialize<List<DepartmentResponseDto>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<DepartmentResponseDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter departamentos: {ex.Message}");
                return new List<DepartmentResponseDto>();
            }
        }

        public async Task<DepartmentResult> CreateDepartmentAsync(CreateDepartmentDto departmentDto)
        {
            try
            {
                var token = await _localStorageService.GetItemAsync<string>("authToken");
                if (string.IsNullOrEmpty(token))
                {
                    return new DepartmentResult
                    {
                        Success = false,
                        ErrorMessage = "Token de autenticação não encontrado."
                    };
                }

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PostAsJsonAsync("www/api/department", departmentDto);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Erro ao criar departamento: {response.StatusCode} - {responseContent}");
                    return new DepartmentResult
                    {
                        Success = false,
                        ErrorMessage = $"Erro ao criar departamento: {responseContent}"
                    };
                }

                var createdDepartment = JsonSerializer.Deserialize<DepartmentResponseDto>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return new DepartmentResult
                {
                    Success = true,
                    Message = "Departamento criado com sucesso.",
                    CreatedDepartment = createdDepartment
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao criar departamento: {ex.Message}");
                return new DepartmentResult
                {
                    Success = false,
                    ErrorMessage = $"Erro ao fazer a requisição: {ex.Message}"
                };
            }
        }

        public async Task<DepartmentResult> DeleteDepartmentAsync(int id)
        {
            try
            {
                var token = await _localStorageService.GetItemAsync<string>("authToken");
                if (string.IsNullOrEmpty(token))
                {
                    return new DepartmentResult
                    {
                        Success = false,
                        ErrorMessage = "Token de autenticação não encontrado."
                    };
                }

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.DeleteAsync($"www/api/department/{id}");
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Erro ao excluir departamento: {response.StatusCode} - {responseContent}");
                    return new DepartmentResult
                    {
                        Success = false,
                        ErrorMessage = $"Erro ao excluir departamento: {responseContent}"
                    };
                }

                return new DepartmentResult
                {
                    Success = true,
                    Message = "Departamento excluído com sucesso."
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao excluir departamento: {ex.Message}");
                return new DepartmentResult
                {
                    Success = false,
                    ErrorMessage = $"Erro ao fazer a requisição: {ex.Message}"
                };
            }
        }

        public async Task<DepartmentResult> UpdateDepartmentAsync(int id, UpdateDepartmentDto departmentDto)
        {
            try
            {
                var token = await _localStorageService.GetItemAsync<string>("authToken");
                if (string.IsNullOrEmpty(token))
                {
                    return new DepartmentResult
                    {
                        Success = false,
                        ErrorMessage = "Token de autenticação não encontrado."
                    };
                }

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PutAsJsonAsync($"www/api/department/{id}", departmentDto);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Erro ao atualizar departamento: {response.StatusCode} - {responseContent}");
                    return new DepartmentResult
                    {
                        Success = false,
                        ErrorMessage = $"Erro ao atualizar departamento: {responseContent}"
                    };
                }

                var updatedDepartment = JsonSerializer.Deserialize<DepartmentResponseDto>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return new DepartmentResult
                {
                    Success = true,
                    Message = "Departamento atualizado com sucesso.",
                    UpdatedDepartment = updatedDepartment
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar departamento: {ex.Message}");
                return new DepartmentResult
                {
                    Success = false,
                    ErrorMessage = $"Erro ao fazer a requisição: {ex.Message}"
                };
            }
        }
    }
}

    

