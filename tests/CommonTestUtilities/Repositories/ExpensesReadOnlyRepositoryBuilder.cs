using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Moq;

namespace CommonTestUtilities.Repositories
{
    public class ExpensesReadOnlyRepositoryBuilder
    {
        private readonly Mock<IExpensesReadOnlyRepository> _repositoryMock;

        public ExpensesReadOnlyRepositoryBuilder()
        {
            _repositoryMock = new Mock<IExpensesReadOnlyRepository>();
        }

        public ExpensesReadOnlyRepositoryBuilder GetAll(User user, List<Expense> expenses)
        {
            _repositoryMock.Setup(repo => repo.GetAll(user)).ReturnsAsync(expenses);
            return this;
        }

        public ExpensesReadOnlyRepositoryBuilder GetById(User user, Expense? expense)
        {
            if(expense is not null)
                _repositoryMock.Setup(repo => repo.GetById(user, expense.Id)).ReturnsAsync(expense);

            return this;
        }

        public IExpensesReadOnlyRepository Build() => _repositoryMock.Object;
    }
}
