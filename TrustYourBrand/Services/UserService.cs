using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using TrustYourBrand.Models;

namespace TrustYourBrand.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;
        private readonly IHttpClientFactory _httpClientFactory;

        public UserService(IHttpClientFactory httpClientFactory, ILocalStorageService localStorageService)
        {
            _httpClient = httpClientFactory.CreateClient("UserApiClient");
            _localStorageService = localStorageService;
        }

        public async Task<List<User>> GetUsers(int? createdByUserId = null)
        {
            try
            {
                var token = await _localStorageService.GetItemAsync<string>("authToken");
                if (string.IsNullOrEmpty(token))
                {
                    Console.WriteLine("Token de autenticação não encontrado.");
                    return new List<User>();
                }

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var query = createdByUserId.HasValue ? $"?createdByUserId={createdByUserId}" : "";
                var response = await _httpClient.GetAsync($"api/user{query}");
                var json = await response.Content.ReadAsStringAsync();

                Console.WriteLine("✅ Conteúdo da resposta da API:");
                Console.WriteLine(json);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"❌ Erro ao obter usuários: {response.StatusCode} - {json}");
                    return new List<User>();
                }

                var apiUsers = JsonSerializer.Deserialize<List<User>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                Console.WriteLine($"✅ {apiUsers?.Count ?? 0} utilizadores carregados da API.");
                return apiUsers ?? new List<User>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao obter usuários: {ex.Message}");
                return new List<User>();
            }
        }


        public async Task<LoginResult> CreateUser(User user)
        {
            try
            {
                if (user == null)
                {
                    return new LoginResult
                    {
                        Success = false,
                        ErrorMessage = "The User object is null."
                    };
                }

                var token = await _localStorageService.GetItemAsync<string>("authToken");
                if (string.IsNullOrEmpty(token))
                {
                    return new LoginResult
                    {
                        Success = false,
                        ErrorMessage = "Authentication token not found."
                    };
                }

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                Console.WriteLine($"Enviando requisição POST para api/user: {JsonSerializer.Serialize(user)}");
                var response = await _httpClient.PostAsJsonAsync("api/user", user);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Resposta do POST: Status {response.StatusCode}, Conteúdo: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ApiCreateUserResponse>();
                    if (result == null || result.UserId == 0)
                    {
                        return new LoginResult
                        {
                            Success = false,
                            ErrorMessage = "API returned an invalid or null result"
                        };
                    }

                    user.Id = result.UserId;
                    return new LoginResult
                    {
                        Success = true,
                        Message = "User created successfully",
                        CreatedUser = user
                    };
                }
                else
                {
                    return new LoginResult
                    {
                        Success = false,
                        ErrorMessage = $"Error creating user: {response.StatusCode} - {responseContent}"
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Exceção no CreateUser: {ex.GetType().Name} - {ex.Message}");
                return new LoginResult
                {
                    Success = false,
                    ErrorMessage = $"Error when making the request: {ex.Message}"
                };
            }
        }

        public async Task<List<RoleDto>> GetRoles()
        {
            try
            {
                var token = await _localStorageService.GetItemAsync<string>("authToken");

                if (string.IsNullOrEmpty(token))
                    return new List<RoleDto>();

                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync("api/user/roles");
                if (!response.IsSuccessStatusCode)
                    return new List<RoleDto>();

                var json = await response.Content.ReadAsStringAsync();

                var rolesDto = JsonSerializer.Deserialize<List<RoleDto>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return rolesDto ?? new List<RoleDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao obter roles: {ex.Message}");
                return new List<RoleDto>();
            }
        }

        public async Task<List<BrandDto>> GetBrands()
        {
            var token = await _localStorageService.GetItemAsync<string>("authToken");
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("No auth token found for GetBrands.");
                return new List<BrandDto>();
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("api/marca");
            response.EnsureSuccessStatusCode();

            var brands = await response.Content.ReadFromJsonAsync<List<BrandDto>>(new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return brands ?? new List<BrandDto>();
        }

        public async Task<List<StoreDto>> GetStores()
        {
            var token = await _localStorageService.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("api/store");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<StoreDto>>();
        }

        public async Task<List<TenantDto>> GetTenants()
        {
            try
            {
                var token = await _localStorageService.GetItemAsync<string>("authToken");
                if (string.IsNullOrEmpty(token))
                {
                    Console.WriteLine("No auth token found for GetTenants.");
                    return new List<TenantDto>();
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync("api/tenant/me"); // Update to the correct endpoint
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Failed to fetch tenant: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                    return new List<TenantDto>();
                }

                var tenant = await response.Content.ReadFromJsonAsync<TenantDto>(new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (tenant == null)
                {
                    Console.WriteLine("No tenant data returned.");
                    return new List<TenantDto>();
                }

                Console.WriteLine($"Fetched tenant: {JsonSerializer.Serialize(tenant)}");
                return new List<TenantDto> { tenant }; // Wrap the single tenant in a list
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching tenant: {ex.Message}");
                return new List<TenantDto>();
            }
        }

        public async Task<LoginResult> UpdateUser(int id, UpdateUserDto userDto)

        {
            try
            {
                var token = await _localStorageService.GetItemAsync<string>("authToken");
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PutAsJsonAsync($"api/user/{id}", userDto);

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return new LoginResult { Success = true, Message = "User edited successfully." };
                }

                return new LoginResult { Success = false, ErrorMessage = content };
            }
            catch (Exception ex)
            {
                return new LoginResult { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<List<DepartmentDto>> GetDepartments()
        {
            try
            {
                var token = await _localStorageService.GetItemAsync<string>("authToken");

                if (string.IsNullOrEmpty(token))
                    return new List<DepartmentDto>();

                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync("api/department");
                if (!response.IsSuccessStatusCode)
                    return new List<DepartmentDto>();

                var json = await response.Content.ReadAsStringAsync();

                var departments = JsonSerializer.Deserialize<List<DepartmentDto>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return departments ?? new List<DepartmentDto>();
            }
            catch
            {
                return new List<DepartmentDto>();
            }
        }

        public async Task<List<FunctionDto>> GetFunctions()
        {
            try
            {
                var token = await _localStorageService.GetItemAsync<string>("authToken");
                if (string.IsNullOrEmpty(token)) return new List<FunctionDto>();

                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync("api/function");
                if (!response.IsSuccessStatusCode) return new List<FunctionDto>();

                var json = await response.Content.ReadAsStringAsync();
                var funcoes = JsonSerializer.Deserialize<List<FunctionDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return funcoes ?? new List<FunctionDto>();
            }
            catch
            {
                return new List<FunctionDto>();
            }
        }



        public async Task<LoginResult> DeactivateUser(int id)
        {
            try
            {
                var token = await _localStorageService.GetItemAsync<string>("authToken");
                Console.WriteLine($"Token obtido: {(string.IsNullOrEmpty(token) ? "Nenhum token" : "Token presente")}");
                if (string.IsNullOrEmpty(token))
                {
                    return new LoginResult
                    {
                        Success = false,
                        ErrorMessage = "Authentication token not found."
                    };
                }

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                Console.WriteLine($"Fazendo requisição DELETE para api/user/{id}");
                var response = await _httpClient.DeleteAsync($"api/user/{id}");
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Resposta recebida: StatusCode={response.StatusCode}, Content={responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    return new LoginResult
                    {
                        Success = true,
                        Message = "User successfully deactivated."
                    };
                }
                else
                {
                    return new LoginResult
                    {
                        Success = false,
                        ErrorMessage = $"Error deactivating user: {response.StatusCode} - {responseContent}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new LoginResult
                {
                    Success = false,
                    ErrorMessage = $"Error when making the request: {ex.Message}"
                };
            }
        }

        public async Task<LoginResult> ChangePin(int userId, string newPin)
        {
            try
            {
                var token = await _localStorageService.GetItemAsync<string>("authToken");
                if (string.IsNullOrEmpty(token))
                {
                    return new LoginResult
                    {
                        Success = false,
                        ErrorMessage = "Authentication token not found."
                    };
                }

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var updatePinDto = new { Pin = newPin };
                var response = await _httpClient.PutAsJsonAsync($"api/user/{userId}", updatePinDto);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"PIN do utilizador {userId} atualizado com sucesso.");
                    return new LoginResult
                    {
                        Success = true,
                        Message = "PIN updated successfully."
                    };
                }
                else
                {
                    return new LoginResult
                    {
                        Success = false,
                        ErrorMessage = $"Error updating PIN: {response.StatusCode} - {responseContent}"
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Exceção no ChangePin: {ex.GetType().Name} - {ex.Message}");
                return new LoginResult
                {
                    Success = false,
                    ErrorMessage = $"Error when making the request: {ex.Message}"
                };
            }
        }
    }


    public class ApiCreateUserResponse
    {
        public int UserId { get; set; }
    }
}