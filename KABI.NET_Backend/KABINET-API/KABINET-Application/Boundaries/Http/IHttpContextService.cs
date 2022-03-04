using Microsoft.AspNetCore.Http;

namespace KABINET_Application.Boundaries.Http
{
    public interface IHttpContextService
    {
        string GetTokenFromHttpContext(HttpContext context);
    }
}
