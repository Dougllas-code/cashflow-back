using CashFlow.Application.UseCases.Expenses.Delete;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.BaseExceptions;
using CommonTestUtilities.Entities.User;
using CommonTestUtilities.LoggerUser;
using CommonTestUtilities.Repositories;

namespace UseCases.Tests.Expenses.Delete
{
    public class DeleteExpensesUseCaseTest
    {

        [Fact]
        public async Task Success()
        {
            // Arrange
            var user = UserBuilder.Build();
            var expense = ExpenseBuilder.Build(user);

            var useCase = CreateUseCase(user, expense);

            // Act
            await useCase.Execute(expense.Id);

            // Assert
            // No exception means success
        }

        [Fact]
        public async Task Error_Expense_Not_Found()
        {
            // Arrange
            var user = UserBuilder.Build();

            var useCase = CreateUseCase(user);

            // Act 
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => useCase.Execute(id: 10000));

            //Assert
            Assert.NotNull(exception);
            Assert.Contains(ResourceErrorMessages.EXPENSE_NOT_FOUND, exception.GetErrors());
            Assert.Single(exception.GetErrors());

        }

        private static DeleteExpenseUseCase CreateUseCase(User user, Expense? expense = null)
        {
            var repository = ExpensesWriteOnlyRepositoryBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(user);
            var readOnlyRepository = new ExpensesReadOnlyRepositoryBuilder().GetById(user, expense).Build();

            return new DeleteExpenseUseCase(repository, unitOfWork, loggedUser, readOnlyRepository);
        }
    }
}
