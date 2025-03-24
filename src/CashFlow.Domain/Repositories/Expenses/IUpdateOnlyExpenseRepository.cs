using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Expenses
{
    public interface IUpdateOnlyExpenseRepository
    {
        void Update(Expense expense);

        Task<Expense?> GetById(long id);
    }
}
