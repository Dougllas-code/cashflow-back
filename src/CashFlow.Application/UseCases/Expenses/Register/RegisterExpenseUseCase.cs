using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Exception.BaseExceptions;

namespace CashFlow.Application.UseCases.Expenses.Register
{
    public class RegisterExpenseUseCase
    {
        public RegisterExpenseResponse Execute(RegisterExpenseRequest request)
        {
            ValidateRequest(request);

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
