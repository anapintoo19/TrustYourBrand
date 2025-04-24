using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TrustYourBrand;
using TrustYourBrand.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using Syncfusion.Blazor;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Registra o AuthTokenHandler como um serviço
builder.Services.AddScoped<AuthTokenHandler>();

// Configura o HttpClient para "ApiClient" com o AuthTokenHandler API LOGIN
builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri("http://ftp.tyb-tst.endinahosting.com/");
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
})
.AddHttpMessageHandler<AuthTokenHandler>();

// Configura o HttpClient para "UserApiClient" com o AuthTokenHandler API USER
builder.Services.AddHttpClient("UserApiClient", client =>
{
    client.BaseAddress = new Uri("http://ftp.tyb-tst.endinahosting.com/");
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
})
.AddHttpMessageHandler<AuthTokenHandler>();

// Configura o HttpClient para "UserApiClient" com o AuthTokenHandler API INSPECTION
builder.Services.AddHttpClient("InspectionApiClient", client =>
{
    client.BaseAddress = new Uri("http://ftp.tyb-tst.endinahosting.com/");
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
})
.AddHttpMessageHandler<AuthTokenHandler>();

// Store api
builder.Services.AddHttpClient("StoreApiClient", client =>
{
    client.BaseAddress = new Uri("http://ftp.tyb-tst.endinahosting.com/");
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
})
.AddHttpMessageHandler<AuthTokenHandler>();

// Department api
builder.Services.AddHttpClient("DepartmentApiClient", client =>
{
    client.BaseAddress = new Uri("http://ftp.tyb-tst.endinahosting.com/");
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
})
.AddHttpMessageHandler<AuthTokenHandler>();

// Tenant api
builder.Services.AddHttpClient("TenantApiClient", client =>
{
    client.BaseAddress = new Uri("http://ftp.tyb-tst.endinahosting.com/");
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
})
.AddHttpMessageHandler<AuthTokenHandler>();

// Brand api
builder.Services.AddHttpClient("BrandApiClient", client =>
{
    client.BaseAddress = new Uri("http://ftp.tyb-tst.endinahosting.com/");
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
})
.AddHttpMessageHandler<AuthTokenHandler>();


// Registra os serviços que dependem do HttpClient
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();
builder.Services.AddScoped<IForgotPinService, ForgotPinService>();
builder.Services.AddScoped<IStoreService, StoreService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IInspectionService, InspectionService>();

builder.Services.AddSyncfusionBlazor();



await builder.Build().RunAsync();