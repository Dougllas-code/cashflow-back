using CashFlow.Communication.Responses;
using CashFlow.Domain.Security.Tokens;

namespace CashFlow.Application.UseCases.CheckToken
{
    public class CheckTokenUseCase : ICheckTokenUseCase
    {
        private readonly ITokenProvider _tokenProvider;
        private readonly IAccessTokenGenerator _tokenGenerator;

        public CheckTokenUseCase(
            ITokenProvider tokenProvider,
            IAccessTokenGenerator tokenGenerator)
        {
            _tokenProvider = tokenProvider;
            _tokenGenerator = tokenGenerator;
        }

        public CheckTokenResponse Execute()
        {
            var token = _tokenProvider.TokenOnRequest();
            var result = _tokenGenerator.IsValid(token);
            return new CheckTokenResponse { IsValid = result };
        }
    }
}
