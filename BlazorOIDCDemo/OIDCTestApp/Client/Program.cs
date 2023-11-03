using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OIDCTestApp.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ApiService>();

builder.Services.AddScoped(sp =>
{
    var handler = ActivatorUtilities.CreateInstance<AuthHandler>(sp);
    handler.InnerHandler ??= new HttpClientHandler();

    return new HttpClient(handler)
    {
        BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
    };
});

builder.Services.AddOidcAuthentication(options =>
{
    options.ProviderOptions.Authority = "https://login.microsoftonline.com/3e3c0505-f852-4e82-a27f-e43f4e47b93d/v2.0";
    options.ProviderOptions.RedirectUri = "https://localhost:7128/authentication/login-callback";
    options.ProviderOptions.PostLogoutRedirectUri = "https://localhost:7128/authentication/logout-callback";
    options.ProviderOptions.ClientId = "6793ce1b-6661-45dc-a324-996dea8aea41";
    options.ProviderOptions.ResponseType = "code";
});

await builder.Build().RunAsync();
