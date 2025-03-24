using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Expenses
{
    public interface IExpensesRepository
    {
        public Task Add(Expense expense);
        public Task<List<Expense>> GetAll();
        public Task<Expense?> GetById(long id);
    }
}
