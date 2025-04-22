using Microsoft.JSInterop;
using System.Text.Json;

namespace TrustYourBrand.Services
{
    public class LocalStorageService : ILocalStorageService
    {
        private readonly IJSRuntime _jsRuntime;

        public LocalStorageService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<T?> GetItemAsync<T>(string key)
        {
            try
            {
                var json = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", key);
                if (string.IsNullOrEmpty(json))
                    return default;

                // Se o tipo esperado é string, retornar diretamente o valor
                if (typeof(T) == typeof(string))
                {
                    return (T)(object)json.Trim('"');
                }

                // Para outros tipos, desserializar o JSON
                return JsonSerializer.Deserialize<T>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter item do localStorage: {ex.Message}");
                return default;
            }
        }

        public async Task SetItemAsync<T>(string key, T value)
        {
            try
            {
                // Se o valor é null, remover o item
                if (value == null)
                {
                    await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
                    return;
                }

                // Se o valor já é uma string, salvar diretamente
                if (typeof(T) == typeof(string))
                {
                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, value);
                }
                else
                {
                    // Para outros tipos, serializar para JSON
                    var json = JsonSerializer.Serialize(value);
                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao definir item no localStorage: {ex.Message}");
            }
        }

        public async Task RemoveItemAsync(string key)
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao remover item do localStorage: {ex.Message}");
            }
        }
    }
}