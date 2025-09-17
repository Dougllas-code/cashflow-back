using CashFlow.Domain.Security.Tokens;

namespace CashFlow.Api.Token
{
    public class HttpContextTokenValue: ITokenProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextTokenValue(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string TokenOnRequest()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
                throw new InvalidOperationException("HttpContext is not available.");

            // Check Authorization header
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
                return authHeader["Bearer ".Length..].Trim();

            // Check query string for access_token (for SignalR)
            var accessToken = context.Request.Query["access_token"].FirstOrDefault();
            if (!string.IsNullOrEmpty(accessToken))
                return accessToken;

            throw new InvalidOperationException("Token not found in the request headers or query string.");
        }
    }
}
