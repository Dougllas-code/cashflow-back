using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses.User;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Security.Criptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Exception.BaseExceptions;

namespace CashFlow.Application.UseCases.Login.DoLogin
{
    public class LoginUseCase : ILoginUseCase
    {
        private readonly IUserReadOnlyRepository _userReadOnlyRepository;
        private readonly IPasswordEncripter _passwordEncripter;
        private readonly IAccessTokenGenerator _accessTokenGenerator;

        public LoginUseCase(
            IUserReadOnlyRepository userReadOnlyRepository,
            IPasswordEncripter passwordEncripter,
            IAccessTokenGenerator accessTokenGenerator
        )
        {
            _userReadOnlyRepository = userReadOnlyRepository;
            _passwordEncripter = passwordEncripter;
            _accessTokenGenerator = accessTokenGenerator;
        }

        public async Task<RegisterUserResponse> Execute(LoginRequest request)
        {
            Validate(request);

            var user = await _userReadOnlyRepository.GetUserByEmail(request.Email);

            if (user is null)
            {
                throw new InvalidLoginException();
            }

            var passwordMatch = _passwordEncripter.Verify(request.Password, user.Password);

            if (!passwordMatch)
            {
                throw new InvalidLoginException();
            }

            return new RegisterUserResponse
            {
                Name = user.Name,
                Token = _accessTokenGenerator.Generate(user)
            };
        }

        private static void Validate(LoginRequest request)
        {
           var result = new LoginValidator().Validate(request);

            if (!result.IsValid)
            {
                var errors = result.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errors);
            }
        }
    }
}
