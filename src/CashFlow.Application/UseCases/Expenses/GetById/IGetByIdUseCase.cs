using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCases.Expenses.GetById
{
    public interface IGetByIdUseCase
    {
        Task<ExpenseResponse> Execute(long id);
    }
}
