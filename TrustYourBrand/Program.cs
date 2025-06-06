﻿using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TrustYourBrand;
using TrustYourBrand.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using Syncfusion.Blazor;
using AKSoftware.Localization.MultiLanguages;
using System.Reflection;
using System.Globalization;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


// Registra o CultureChangeService
builder.Services.AddSingleton<CultureChangeService>();

builder.Services.AddLanguageContainer(Assembly.GetExecutingAssembly(), CultureInfo.GetCultureInfo("en-US"));
Console.WriteLine("LanguageContainer configurado com cultura padrão: en-US");
//builder.Services.AddLanguageContainer(Assembly.GetExecutingAssembly();

// Registra o AuthTokenHandler como um serviço
builder.Services.AddScoped<AuthTokenHandler>();

// Configura o HttpClient para "ApiClient" com o AuthTokenHandler API LOGIN
builder.Services.AddHttpClient("ApiClient", client =>
{
    //client.BaseAddress = new Uri("https://tyb-tst.endinahosting.com/");
    //client.BaseAddress = new Uri("http://localhost:5097/");
    client.BaseAddress = new Uri("https://localhost:7102/");
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
})
.AddHttpMessageHandler<AuthTokenHandler>();

// Configura o HttpClient para "UserApiClient" com o AuthTokenHandler API USER
builder.Services.AddHttpClient("UserApiClient", client =>
{
    //client.BaseAddress = new Uri("http://tyb-tst.endinahosting.com/");
    client.BaseAddress = new Uri("https://localhost:7102/");
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
})
.AddHttpMessageHandler<AuthTokenHandler>();

// Configura o HttpClient para "UserApiClient" com o AuthTokenHandler API INSPECTION
builder.Services.AddHttpClient("InspectionApiClient", client =>
{
    //client.BaseAddress = new Uri("https://tyb-tst.endinahosting.com/");
    client.BaseAddress = new Uri("https://localhost:7102/");
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
})
.AddHttpMessageHandler<AuthTokenHandler>();

// Store api
builder.Services.AddHttpClient("StoreApiClient", client =>
{
    //client.BaseAddress = new Uri("https://tyb-tst.endinahosting.com/");
    client.BaseAddress = new Uri("https://localhost:7102/");
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
})
.AddHttpMessageHandler<AuthTokenHandler>();

// Department api
builder.Services.AddHttpClient("DepartmentApiClient", client =>
{
    //client.BaseAddress = new Uri("https://tyb-tst.endinahosting.com/");
    client.BaseAddress = new Uri("https://localhost:7102/");
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
})
.AddHttpMessageHandler<AuthTokenHandler>();

// Tenant api
builder.Services.AddHttpClient("TenantApiClient", client =>
{
    //client.BaseAddress = new Uri("https://tyb-tst.endinahosting.com/");
    client.BaseAddress = new Uri("https://localhost:7102/");
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
})
.AddHttpMessageHandler<AuthTokenHandler>();

// Brand api
builder.Services.AddHttpClient("BrandApiClient", client =>
{
    //client.BaseAddress = new Uri("https://tyb-tst.endinahosting.com/");
    client.BaseAddress = new Uri("https://localhost:7102/");
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
})
.AddHttpMessageHandler<AuthTokenHandler>();


// Registra os servi�os que dependem do HttpClient
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();
builder.Services.AddScoped<IForgotPinService, ForgotPinService>();
builder.Services.AddScoped<IStoreService, StoreService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IInspectionService, InspectionService>();
builder.Services.AddScoped<ITemplateService, TemplateService>();

builder.Services.AddSyncfusionBlazor();

var host = builder.Build();

// Carrega a cultura salva do LocalStorage
var localStorageService = host.Services.GetRequiredService<ILocalStorageService>();
var languageContainer = host.Services.GetRequiredService<ILanguageContainerService>();

try
{
    var savedCulture = await localStorageService.GetItemAsync<string>("culture");
    Console.WriteLine($"Cultura salva no LocalStorage: {savedCulture ?? "Nenhuma"}");

    var supportedCultures = new[] { "en-US", "pt-PT", "es-ES" };
    var culture = supportedCultures.Contains(savedCulture) ? savedCulture : "en-US";
    Console.WriteLine($"Cultura selecionada: {culture}");

    languageContainer.SetLanguage(CultureInfo.GetCultureInfo(culture));
    CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(culture);
    CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(culture);

    // Log manual das culturas suportadas
    Console.WriteLine("Culturas disponíveis: " + string.Join(", ", supportedCultures));
}
catch (Exception ex)
{
    Console.WriteLine($"Erro ao carregar cultura do LocalStorage: {ex.Message}");
}

await host.RunAsync();