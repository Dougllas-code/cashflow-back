using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses.User;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Security.Criptography;
using CashFlow.Exception;
using CashFlow.Exception.BaseExceptions;
using FluentValidation.Results;

namespace CashFlow.Application.UseCases.User.Register
{
    public class RegisterUserUseCase : IRegisterUserUseCase
    {
        private IMapper _mapper;
        private IPasswordEncripter _passwordEncripter;
        private IUserReadOnlyRepository _userReadOnlyRepository;
        private IUserWriteOnlyRepository _userWriteOnlyRepository;
        private IUnitOfWork _unitOfWork;

        public RegisterUserUseCase(
            IMapper mapper,
            IPasswordEncripter passwordEncripter,
            IUserReadOnlyRepository userReadOnlyRepository,
            IUserWriteOnlyRepository userWriteOnlyRepository,
            IUnitOfWork unitOfWork

        )
        {
            _mapper = mapper;
            _passwordEncripter = passwordEncripter;
            _userReadOnlyRepository = userReadOnlyRepository;
            _userWriteOnlyRepository = userWriteOnlyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RegisterUserResponse> Execute(UserRequest request)
        {
            await Validate(request);

            var user = _mapper.Map<Domain.Entities.User>(request);
            user.Password = _passwordEncripter.Encrypt(request.Password);
            user.UserIdentifier = Guid.NewGuid();

            await _userWriteOnlyRepository.Add(user);
            await _unitOfWork.Commit();

            return new RegisterUserResponse
            {
                Name = user.Name,
            };
        }

        private async Task Validate(UserRequest request)
        {
            var result = new UserValidator().Validate(request);
            var emailExist = await _userReadOnlyRepository.ExistActiveUserWithEmail(request.Email);

            if (emailExist)
            {
                result.Errors.Add(new ValidationFailure(
                    nameof(request.Email),
                    ResourceErrorMessages.EMAIL_ALREADY_REGISTERED
                ));
            }

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
