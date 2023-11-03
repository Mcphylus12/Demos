using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace OIDCTestApp.Client;

public class AuthHandler : AuthorizationMessageHandler
{
    public AuthHandler(IAccessTokenProvider provider, NavigationManager navigation) 
        : base(provider, navigation)
    {
        ConfigureHandler(new[] { "https://localhost:7128/" }, new[] { "api://test-api/test1" });
        InnerHandler = new HttpClientHandler();
    }
}
