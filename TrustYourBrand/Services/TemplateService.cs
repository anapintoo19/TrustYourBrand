using Blazored.LocalStorage;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using TrustYourBrand.Models;
using TrustYourBrand.Pages;

namespace TrustYourBrand.Services
{
    public class TemplateService : ITemplateService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;
        public TemplateService(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }
        public async Task<List<TemplateDto>> GetTemplates()
        {
            var token = await _localStorageService.GetItemAsync<string>("authToken");
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("No auth token found for GetTemplates.");
                return new List<TemplateDto>();
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //var response = await _httpClient.GetAsync("www/api/formulario");
            var response = await _httpClient.GetAsync("https://localhost:7102/api/formulario");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"GetTemplates API Response: {json}"); // Log da resposta

            var templates = await response.Content.ReadFromJsonAsync<List<TemplateDto>>(new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Console.WriteLine($"Loaded {templates?.Count ?? 0} templates.");
            return templates ?? new List<TemplateDto>();
        }

        public async Task<List<CustomQuestion>> GetTemplateQuestions(int templateId)
        {
            try
            {
                //var formularios = await _httpClient.GetFromJsonAsync<List<FormularioResponse>>("www/api/formulario");
                var formularios = await _httpClient.GetFromJsonAsync<List<FormularioResponse>>("https://localhost:7102/api/formulario");
                Console.WriteLine($"API Response: {JsonSerializer.Serialize(formularios)}");

                var formulario = formularios?.FirstOrDefault(f => f.FormularioId == templateId);

                if (formulario == null || formulario.Perguntas == null)
                {
                    Console.WriteLine($"No questions found for template ID {templateId}");
                    return new List<CustomQuestion>();
                }

                var questions = formulario.Perguntas.Select(p => new CustomQuestion
                {
                    Id = p.Id,
                    TemplateId = templateId,
                    SeccaoId = p.SeccaoId,
                    Text = p.Texto,
                    ResponseType = p.TipoResposta,
                    Options = p.Opcoes?.ToList() ?? new List<string>(),
                    Resposta = p.Resposta
                }).ToList();

                Console.WriteLine($"Loaded {questions.Count} questions for template ID {templateId}");
                return questions;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching template questions: {ex.Message}");
                return new List<CustomQuestion>();
            }
        }
    }
}
