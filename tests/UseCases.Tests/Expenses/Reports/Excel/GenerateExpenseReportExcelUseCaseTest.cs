using CashFlow.Application.UseCases.Expenses.Report.Excel;
using CashFlow.Domain.Entities;
using CommonTestUtilities.Entities.User;
using CommonTestUtilities.LoggerUser;
using CommonTestUtilities.Repositories;

namespace UseCases.Tests.Expenses.Reports.Excel
{
    public class GenerateExpenseReportExcelUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            // Arrange
            var user = UserBuilder.Build();
            var expenses = ExpenseBuilder.Collection(user);

            var useCase = CreateUseCase(user, expenses);

            var month = DateOnly.FromDateTime(DateTime.Today);

            // Act
            var result = await useCase.Execute(month);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<byte[]>(result);
        }

        [Fact]
        public async Task Success_Empty()
        {
            // Arrange
            var user = UserBuilder.Build();
            var expenses = new List<Expense>();

            var useCase = CreateUseCase(user, expenses);

            var month = DateOnly.FromDateTime(DateTime.Today);

            // Act
            var result = await useCase.Execute(month);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            Assert.IsType<byte[]>(result);
        }

        private static GenerateExpensesReportExcelUseCase CreateUseCase(User user, List<Expense> expenses)
        {
            var repository = new ExpensesReadOnlyRepositoryBuilder().GetByMonth(user, expenses).Build();
            var loggedUser = LoggedUserBuilder.Build(user);

            return new GenerateExpensesReportExcelUseCase(repository, loggedUser);
        }
    }
}
