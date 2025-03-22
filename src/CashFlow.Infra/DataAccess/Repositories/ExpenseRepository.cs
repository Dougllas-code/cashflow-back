using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;

namespace CashFlow.Infra.DataAccess.Repositories
{
    internal class ExpenseRepository : IExpensesRepository
    {
        private readonly CashFlowDbContext _dbContext;

        public ExpenseRepository(CashFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(Expense expense)
        {
            await _dbContext.Expenses.AddAsync(expense);
        }
    }
}
