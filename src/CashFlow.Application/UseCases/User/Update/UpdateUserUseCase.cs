using CashFlow.Communication.Requests;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.BaseExceptions;
using FluentValidation.Results;

namespace CashFlow.Application.UseCases.User.Update
{
    public class UpdateUserUseCase : IUpdateUserUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IUserReadOnlyRepository _readOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserUpdateOnlyRepository _updateOnlyRepository;

        public UpdateUserUseCase(
            ILoggedUser loggedUser,
            IUserReadOnlyRepository readOnlyRepository,
            IUnitOfWork unitOfWork,
            IUserUpdateOnlyRepository updateOnlyRepository)
        {
            _loggedUser = loggedUser;
            _readOnlyRepository = readOnlyRepository;
            _unitOfWork = unitOfWork;
            _updateOnlyRepository = updateOnlyRepository;
        }

        public async Task Execute(UpdateUserRequest request)
        {
            var loggedUser = await _loggedUser.Get();

            await Validate(request, loggedUser.Email);

            var user = await _updateOnlyRepository.GetById(loggedUser.Id);

            user.Name = request.Name;
            user.Email = request.Email;

            _updateOnlyRepository.Update(user);

            await _unitOfWork.Commit();
        }

        private async Task Validate(UpdateUserRequest request, string currentEmail)
        {
            var result = new UpdateUserValidator().Validate(request);

            if (!currentEmail.Equals(request.Email))
            {
                var userExists = await _readOnlyRepository.ExistActiveUserWithEmail(request.Email);
                if (userExists)
                    result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));
            }

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
