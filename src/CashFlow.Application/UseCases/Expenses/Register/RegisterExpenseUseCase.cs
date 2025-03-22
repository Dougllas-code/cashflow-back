using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception.BaseExceptions;

namespace CashFlow.Application.UseCases.Expenses.Register
{
    public class RegisterExpenseUseCase : IRegisterExpenseUseCase
    {

        private readonly IExpensesRepository _expenseRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterExpenseUseCase(IExpensesRepository expenseRepository, IUnitOfWork unitOfWork)
        {
            _expenseRepository = expenseRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RegisterExpenseResponse> Execute(RegisterExpenseRequest request)
        {
            ValidateRequest(request);

            var entity = new Domain.Entities.Expense
            {
                Title = request.Title,
                Description = request.Description,
                Date = request.Date,
                Amount = request.Amount,
                PaymentType = (Domain.Enums.PaymentType)request.PaymentType
            };

            await _expenseRepository.Add(entity);
            await _unitOfWork.Commit();

            return new RegisterExpenseResponse(request.Title);
        }

        private static void ValidateRequest(RegisterExpenseRequest request)
        {
            var result = new RegisterExpenseValidator().Validate(request);
            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
