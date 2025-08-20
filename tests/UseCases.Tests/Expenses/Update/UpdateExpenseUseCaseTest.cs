using CashFlow.Application.UseCases.Expenses.Update;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.BaseExceptions;
using CommonTestUtilities.Entities.User;
using CommonTestUtilities.InlineData;
using CommonTestUtilities.LoggerUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using System;

namespace UseCases.Tests.Expenses.Update
{
    public class UpdateExpenseUseCaseTest
    {

        [Fact]
        public async Task Success()
        {
            //Arrange
            var request = ExpenseRequestBuilder.Build();
            var user = UserBuilder.Build();
            var expense = ExpenseBuilder.Build(user);

            var useCase = CreateUseCase(user, expense);

            //Act
            await useCase.Execute(expense.Id, request);

            //Assert
            Assert.Equal(request.Title, expense.Title);
            Assert.Equal(request.Description, expense.Description);
            Assert.Equal(request.Date, expense.Date);
            Assert.Equal(request.Amount, expense.Amount);
            Assert.Equal((int)request.PaymentType, (int)expense.PaymentType);
        }

        [Theory]
        [ClassData(typeof(InvalidStringInlineData))]
        public async Task Error_Title_Empty(string title)
        {
            //Arrange
            var request = ExpenseRequestBuilder.Build();
            request.Title = title;

            var user = UserBuilder.Build();
            var expense = ExpenseBuilder.Build(user);

            var useCase = CreateUseCase(user, expense);

            //Act & Assert
            var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(() => useCase.Execute(expense.Id, request));

            //Assert
            Assert.NotNull(exception);
            Assert.Contains(ResourceErrorMessages.TITLE_REQUIRED, exception.GetErrors());
            Assert.Single(exception.GetErrors());
        }

        [Fact]
        public async Task Error_Expense_Not_Found()
        {
            //Arrange
            var user = UserBuilder.Build();
            var request = ExpenseRequestBuilder.Build();
            var useCase = CreateUseCase(user);

            //Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => useCase.Execute(id: 10000, request));

            //Assert
            Assert.NotNull(exception);
            Assert.Contains(ResourceErrorMessages.EXPENSE_NOT_FOUND, exception.GetErrors());
            Assert.Single(exception.GetErrors());
        }

        private static UpdateExpenseUseCase CreateUseCase(User user, Expense? expense = null)
        {
            var repository = new ExpensesUpdateOnlyRepositoryBuilder().GetById(user, expense).Build();
            var mapper = MapperBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(user);

            return new UpdateExpenseUseCase(repository, mapper, unitOfWork, loggedUser);

        }
    }
}
