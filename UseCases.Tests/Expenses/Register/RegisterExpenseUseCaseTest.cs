using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.BaseExceptions;
using CommonTestUtilities.Entities.User;
using CommonTestUtilities.InlineData;
using CommonTestUtilities.LoggerUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;

namespace UseCases.Tests.Expenses.Register
{
    public class RegisterExpenseUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            // Arrange
            var user = UserBuilder.Build();
            var request = RegisterExpenseRequestBuilder.Build();
            var useCase = CreateUseCase(user);

            // Act
            var result = await useCase.Execute(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.Title, request.Title);
        }

        [Theory]
        [ClassData(typeof(InvalidStringInlineData))]
        public async Task Error_Title_Empty(string title)
        {
            // Arrange
            var user = UserBuilder.Build();
            var request = RegisterExpenseRequestBuilder.Build();
            request.Title = title;

            var useCase = CreateUseCase(user);

            // Act
            var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(() => useCase.Execute(request));

            //Assert
            Assert.NotNull(exception);
            Assert.Contains(ResourceErrorMessages.TITLE_REQUIRED, exception.GetErrors());
            Assert.Single(exception.GetErrors());
        }

        private static RegisterExpenseUseCase CreateUseCase(User user)
        {
            var writeOnlyRepository = ExpensesWriteOnlyRepositoryBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var mapper = MapperBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(user);

            return new RegisterExpenseUseCase(writeOnlyRepository, unitOfWork, mapper, loggedUser);
        }
    }
}
