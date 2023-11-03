using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace OIDCTestApp.Server.Controllers;

[ApiController]
[Authorize(Policy = "ValidUser")]
public class UserEndpoint : ControllerBase
{
    [HttpGet]
    [Route("/me")]
    public IEnumerable<string> GetUserInfo()
    {
        return User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
    }
}
