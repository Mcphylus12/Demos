using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using OIDCTestApp.Shared;
using System.Net.Http.Json;

namespace OIDCTestApp.Client;

public class ApiService
{
    private readonly HttpClient httpClient;

    public ApiService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<WeatherForecast[]?> CallWeather()
    {
        try
        {
            return await httpClient.GetFromJsonAsync<WeatherForecast[]>("WeatherForecast");
        }
        catch (AccessTokenNotAvailableException exception)
        {
            exception.Redirect();
            return Array.Empty<WeatherForecast>();
        }
    }

    private static Task<string[]?>? _infoTask = null;

    public async Task<string[]?> UserInfo()
    {
        try
        {
            if (_infoTask is null)
            {
                _infoTask = httpClient.GetFromJsonAsync<string[]>("me");
            }

            return await _infoTask;
        }
        catch (AccessTokenNotAvailableException exception)
        {
            exception.Redirect();
            return Array.Empty<string>();
        }
    }
}
