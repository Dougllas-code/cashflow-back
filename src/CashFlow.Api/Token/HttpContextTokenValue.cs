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
            var token = _httpContextAccessor.HttpContext!.Request.Headers.Authorization.ToString();

            if (string.IsNullOrEmpty(token))
                throw new InvalidOperationException("Token not found in the request headers.");

            return token["Bearer ".Length..].Trim();
        }
    }
}
