using CashFlow.Application.UseCases.Expenses.GetAll;
using CashFlow.Domain.Entities;
using CommonTestUtilities.Entities.User;
using CommonTestUtilities.LoggerUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;

namespace UseCases.Tests.Expenses.GetAll
{
    public class GetAllExpensesUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            var user = UserBuilder.Build();
            var expenses = ExpenseBuilder.Collection(user);

            var useCase = CreateUseCase(user, expenses);

            //Act
            var result = await useCase.Execute();

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Expenses);
            Assert.Equal(expenses.Count, result.Expenses.Count); 
            Assert.All(result.Expenses, expense => 
            {
                Assert.NotNull(expense);
                Assert.NotEqual(0, expense.Id);
                Assert.NotNull(expense.Title);
                Assert.NotEmpty(expense.Title);
                Assert.True(expense.Amount > 0);
            });
        }

        private static GetAllExpensesUseCase CreateUseCase(User user, List<Expense> expenses) 
        { 
            var repository = new ExpensesReadOnlyRepositoryBuilder().GetAll(user, expenses).Build();
            var mapper = MapperBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(user);

            return new GetAllExpensesUseCase(repository, mapper, loggedUser);
        }
    }
}
