using Microsoft.AspNetCore.Components.Authorization;

namespace OIDCTestApp.Client;

public class UserService
{
    private readonly AuthenticationStateProvider authenticationStateProvider;
    private readonly ApiService apiService;
    private static User? _cachedUser;
    public event Func<User, Task>? OnAuthChange;

    public UserService(AuthenticationStateProvider authenticationStateProvider, ApiService apiService)
    {
        this.authenticationStateProvider = authenticationStateProvider;
        this.apiService = apiService;
        authenticationStateProvider.AuthenticationStateChanged += ResetState;
    }

    private async void ResetState(Task<AuthenticationState> task)
    {
        _cachedUser = null;
        var user = await GetUser();
        OnAuthChange?.Invoke(user);
    }

    public async Task<User> GetUser()
    {
        if (_cachedUser is not null)
        {
            return _cachedUser;
        }

        var authState = await authenticationStateProvider.GetAuthenticationStateAsync();

        if (authState?.User?.Identity is null || !authState.User.Identity.IsAuthenticated) 
        {
            _cachedUser = new User();
            return _cachedUser;
        }

        var roles = await apiService.UserInfo();

        _cachedUser = new User()
        {
            Roles = roles!
        };
        return _cachedUser;
    }
}

public class User
{
    public string[] Roles { get; set; }
}
