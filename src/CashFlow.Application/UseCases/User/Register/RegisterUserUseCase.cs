using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses.User;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Security.Criptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Exception;
using CashFlow.Exception.BaseExceptions;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;

namespace CashFlow.Application.UseCases.User.Register
{
    public class RegisterUserUseCase : IRegisterUserUseCase
    {
        private readonly IMapper _mapper;
        private readonly IPasswordEncripter _passwordEncripter;
        private readonly IUserReadOnlyRepository _userReadOnlyRepository;
        private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccessTokenGenerator _accessTokenGenerator;
        private readonly ILogger<RegisterUserUseCase> _logger;

        public RegisterUserUseCase(
            IMapper mapper,
            IPasswordEncripter passwordEncripter,
            IUserReadOnlyRepository userReadOnlyRepository,
            IUserWriteOnlyRepository userWriteOnlyRepository,
            IUnitOfWork unitOfWork,
            IAccessTokenGenerator accessTokenGenerator,
            ILogger<RegisterUserUseCase> logger
        )
        {
            _mapper = mapper;
            _passwordEncripter = passwordEncripter;
            _userReadOnlyRepository = userReadOnlyRepository;
            _userWriteOnlyRepository = userWriteOnlyRepository;
            _unitOfWork = unitOfWork;
            _accessTokenGenerator = accessTokenGenerator;
            _logger = logger;
        }

        public async Task<RegisterUserResponse> Execute(UserRequest request)
        {
            _logger.LogInformation("Starting user registration process for email: {Email}", request.Email);

            await Validate(request);
            _logger.LogDebug("User registration validation passed for email: {Email}", request.Email);

            var user = _mapper.Map<Domain.Entities.User>(request);
            user.Password = _passwordEncripter.Encrypt(request.Password);
            user.UserIdentifier = Guid.NewGuid();

            _logger.LogDebug("User entity created with UserIdentifier: {UserIdentifier} for email: {Email}", user.UserIdentifier, request.Email);

            await _userWriteOnlyRepository.Add(user);
            _logger.LogDebug("User added to repository for email: {Email}", request.Email);

            await _unitOfWork.Commit();
            _logger.LogDebug("User registration committed to database for email: {Email}", request.Email);

            _logger.LogInformation("User registration successful for email: {Email}, UserID: {UserId}. Token generated.", request.Email, user.Id);

            return new RegisterUserResponse
            {
                Name = user.Name,
                Token = _accessTokenGenerator.Generate(user)
            };
        }

        private async Task Validate(UserRequest request)
        {
            var result = new UserValidator().Validate(request);
            var emailExist = await _userReadOnlyRepository.ExistActiveUserWithEmail(request.Email);

            if (emailExist)
            {
                _logger.LogWarning("User registration failed - Email already exists: {Email}", request.Email);
                result.Errors.Add(new ValidationFailure(
                    nameof(request.Email),
                    ResourceErrorMessages.EMAIL_ALREADY_REGISTERED
                ));
            }

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
                _logger.LogError("Validation error during user registration for email: {Email}. Errors: {ValidationErrors}", 
                    request.Email, string.Join(", ", errorMessages));
                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
