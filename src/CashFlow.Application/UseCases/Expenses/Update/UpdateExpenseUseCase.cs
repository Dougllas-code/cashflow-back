using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.BaseExceptions;

namespace CashFlow.Application.UseCases.Expenses.Update
{
    public class UpdateExpenseUseCase : IUpdateExpenseUseCase
    {
        private readonly IExpensesUpdateOnlyRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggedUser _loggedUser;

        public UpdateExpenseUseCase(
            IExpensesUpdateOnlyRepository repository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ILoggedUser loggedUser)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _loggedUser = loggedUser;
        }

        public async Task Execute(long id, ExpenseRequest request)
        {
            ValidateRequest(request);

            var loggedUser = await _loggedUser.Get();

            var expense = await _repository.GetById(loggedUser, id);

            if (expense is null)
            {
                throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
            }

            var result = _mapper.Map(source: request, destination: expense);
            expense.Id = id;

            _repository.Update(result);
            await _unitOfWork.Commit();
        }

        private static void ValidateRequest(ExpenseRequest request)
        {
            var result = new ExpenseValidator().Validate(request);
            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
