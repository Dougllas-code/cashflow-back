using CashFlow.Application.UseCases.Expenses.GetById;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.BaseExceptions;
using CommonTestUtilities.Entities.User;
using CommonTestUtilities.LoggerUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;

namespace UseCases.Tests.Expenses.GetById
{
    public class GetExpenseByIdUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            // Arrange
            var user = UserBuilder.Build();
            var expense = ExpenseBuilder.Build(user);

            var useCase = CreateUseCase(user, expense);

            //Act
            var result = await useCase.Execute(expense.Id);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(expense.Id, result.Id);
            Assert.Equal(expense.Title, result.Title);
            Assert.Equal(expense.Description, result.Description);
            Assert.Equal(expense.Date, result.Date);
            Assert.Equal(expense.Amount, result.Amount);
            Assert.Equal(expense.PaymentType, (CashFlow.Domain.Enums.PaymentType)result.PaymentType);
        }

        [Fact]
        public async Task Error_Expense_Not_Found()
        {
            // Arrange
            var user = UserBuilder.Build();

            var useCase = CreateUseCase(user);

            //Act
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => useCase.Execute(id: 10000));

            //Assert
            Assert.NotNull(exception);
            Assert.Contains(ResourceErrorMessages.EXPENSE_NOT_FOUND, exception.GetErrors());
            Assert.Single(exception.GetErrors());
        }

        private GetByIdUseCase CreateUseCase(User user, Expense? expense = null)
        {
            var repository = new ExpensesReadOnlyRepositoryBuilder().GetById(user, expense).Build();
            var mapper = MapperBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(user);

            return new GetByIdUseCase(repository, mapper, loggedUser);
        }
    }
}
