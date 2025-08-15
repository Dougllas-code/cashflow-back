using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Moq;

namespace CommonTestUtilities.Repositories
{
    public class ExpensesUpdateOnlyRepositoryBuilder
    {
        private readonly Mock<IExpensesUpdateOnlyRepository> _repositoryMock;

        public ExpensesUpdateOnlyRepositoryBuilder()
        {
            _repositoryMock = new Mock<IExpensesUpdateOnlyRepository>();
        }

        public ExpensesUpdateOnlyRepositoryBuilder GetById(User user, Expense? expense)
        {
            if (expense is not null)
                _repositoryMock.Setup(repo => repo.GetById(user, expense.Id)).ReturnsAsync(expense);

            return this;
        }

        public IExpensesUpdateOnlyRepository Build() => _repositoryMock.Object;
    }
}
