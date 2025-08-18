using CashFlow.Communication.Requests;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Security.Criptography;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.BaseExceptions;
using FluentValidation.Results;

namespace CashFlow.Application.UseCases.User.ChangePassword
{
    public class ChangePasswordUseCase : IChangePasswordUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IUserUpdateOnlyRepository _updateRepository;
        private readonly IPasswordEncripter _passwordEncripter;
        private readonly IUnitOfWork _unitOfWork;

        public ChangePasswordUseCase(
            ILoggedUser loggedUser,
            IUserUpdateOnlyRepository updateRepository,
            IPasswordEncripter passwordEncripter,
            IUnitOfWork unitOfWork)
        {
            _loggedUser = loggedUser;
            _updateRepository = updateRepository;
            _passwordEncripter = passwordEncripter;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(ChangePasswordRequest request)
        {
            var loggedUser = await _loggedUser.Get();

            Validate(request, loggedUser);

            var user = await _updateRepository.GetById(loggedUser.Id);

            user.Password = _passwordEncripter.Encrypt(request.NewPassword);

            _updateRepository.Update(user);

            await _unitOfWork.Commit();
        }

        private void Validate(ChangePasswordRequest request, Domain.Entities.User loggedUser)
        {
            var result = new ChangePasswordValidator().Validate(request);

            var passwordMatch = _passwordEncripter.Verify(request.CurrentPassword, loggedUser.Password);

            if (!passwordMatch)
            {
                result.Errors.Add(new ValidationFailure(
                    nameof(request.CurrentPassword),
                    ResourceErrorMessages.CURRENT_PASSWORD_INCORRECT
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
