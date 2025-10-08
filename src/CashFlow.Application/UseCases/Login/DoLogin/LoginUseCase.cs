using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses.User;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Security.Criptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Exception.BaseExceptions;
using Microsoft.Extensions.Logging;

namespace CashFlow.Application.UseCases.Login.DoLogin
{
    public class LoginUseCase : ILoginUseCase
    {
        private readonly IUserReadOnlyRepository _userReadOnlyRepository;
        private readonly IPasswordEncripter _passwordEncripter;
        private readonly IAccessTokenGenerator _accessTokenGenerator;
        private readonly ILogger<LoginUseCase> _logger;

        public LoginUseCase(
            IUserReadOnlyRepository userReadOnlyRepository,
            IPasswordEncripter passwordEncripter,
            IAccessTokenGenerator accessTokenGenerator,
            ILogger<LoginUseCase> logger
        )
        {
            _userReadOnlyRepository = userReadOnlyRepository;
            _passwordEncripter = passwordEncripter;
            _accessTokenGenerator = accessTokenGenerator;
            _logger = logger;
        }

        public async Task<RegisterUserResponse> Execute(LoginRequest request)
        {
            _logger.LogInformation("Starting login process for email: {Email}", request.Email);

            Validate(request);
            _logger.LogDebug("Login request validation passed for email: {Email}", request.Email);

            var user = await _userReadOnlyRepository.GetUserByEmail(request.Email);

            if (user is null)
            {
                _logger.LogWarning("Login attempt failed - User not found for email: {Email}", request.Email);
                throw new InvalidLoginException();
            }

            _logger.LogDebug("User found for email: {Email}, UserID: {UserId}", request.Email, user.Id);

            var passwordMatch = _passwordEncripter.Verify(request.Password, user.Password);

            if (!passwordMatch)
            {
                _logger.LogWarning("Login attempt failed - Invalid password for email: {Email}, UserID: {UserId}", request.Email, user.Id);
                throw new InvalidLoginException();
            }

            _logger.LogDebug("Password verification successful for email: {Email}, UserID: {UserId}", request.Email, user.Id);

            var token = _accessTokenGenerator.Generate(user);
            _logger.LogInformation("Login successful for email: {Email}, UserID: {UserId}. Token generated.", request.Email, user.Id);

            return new RegisterUserResponse
            {
                Name = user.Name,
                Token = token
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
