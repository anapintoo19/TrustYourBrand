using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using TrustYourBrand.Services; // Adiciona este using para usar o JwtHelper

namespace TrustYourBrand.Services
{
    public class AuthTokenHandler : DelegatingHandler
    {
        private readonly ILocalStorageService _localStorageService;

        public AuthTokenHandler(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _localStorageService.GetItemAsync<string>("authToken");
            Console.WriteLine($"Token obtido no AuthTokenHandler: '{token}'");

            if (!string.IsNullOrEmpty(token) && !JwtHelper.IsTokenExpired(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                Console.WriteLine("Token adicionado ao cabeçalho Authorization.");
            }
            else
            {
                if (string.IsNullOrEmpty(token))
                {
                    Console.WriteLine("Nenhum token encontrado no localStorage.");
                }
                else
                {
                    Console.WriteLine("⚠ Token expirado. Não foi adicionado ao cabeçalho Authorization.");
                }
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}