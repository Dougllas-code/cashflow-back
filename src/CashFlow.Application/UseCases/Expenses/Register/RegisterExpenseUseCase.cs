using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses.Expenses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception.BaseExceptions;

namespace CashFlow.Application.UseCases.Expenses.Register
{
    internal class RegisterExpenseUseCase : IRegisterExpenseUseCase
    {

        private readonly IExpensesWriteOnlyRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILoggedUser _loggedUser;

        public RegisterExpenseUseCase(
            IExpensesWriteOnlyRepository repository, 
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILoggedUser loggedUser)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _loggedUser = loggedUser;
        }

        public async Task<RegisterExpenseResponse> Execute(ExpenseRequest request )
        {
            ValidateRequest(request);

            var loggedUser = await _loggedUser.Get();

            var expense = _mapper.Map<Expense>(request);
            expense.UserId = loggedUser.Id;

            await _repository.Add(expense);
            await _unitOfWork.Commit();

            return _mapper.Map<RegisterExpenseResponse>(expense);
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
