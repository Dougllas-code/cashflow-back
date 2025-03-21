using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception.BaseExceptions;

namespace CashFlow.Application.UseCases.Expenses.Register
{
    public class RegisterExpenseUseCase : IRegisterExpenseUseCase
    {

        private readonly IExpensesRepository _expenseRepository;

        public RegisterExpenseUseCase(IExpensesRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        public RegisterExpenseResponse Execute(RegisterExpenseRequest request)
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

            _expenseRepository.Add(entity);
            return new RegisterExpenseResponse();
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
