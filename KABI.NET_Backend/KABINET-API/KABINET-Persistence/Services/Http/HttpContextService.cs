using KABINET_Application.Boundaries.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Linq;

namespace KABINET_Persistence.Services.Http
{
    public class HttpContextService : IHttpContextService
    {
        public string GetTokenFromHttpContext(HttpContext context)
        {
            bool userHeaderExists = context.Request.Headers.TryGetValue("X-AccessToken", out StringValues userHeader);
            var token = userHeaderExists ? userHeader.First() : null;

            return token;
        }
    }
}
