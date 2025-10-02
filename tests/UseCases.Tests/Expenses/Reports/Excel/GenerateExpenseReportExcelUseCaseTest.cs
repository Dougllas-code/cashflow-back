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
        }

        [Fact]
        public async Task Success_Empty()
        {
            // Arrange
            var user = UserBuilder.Build();

            var useCase = CreateUseCase(user, []);

            var month = DateOnly.FromDateTime(DateTime.Today);

            // Act
            var result = await useCase.Execute(month);

            // Assert
            Assert.Null(result);
        }

        private static GenerateExpensesReportExcelUseCase CreateUseCase(User user, List<Expense> expenses)
        {
            var repository = new ExpensesReadOnlyRepositoryBuilder().GetByMonth(user, expenses).Build();
            var loggedUser = LoggedUserBuilder.Build(user);
            var reportRequestRepository = ReportRequestRepositoryBuilder.Build();
            var messageBus = MessageBusBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();

            return new GenerateExpensesReportExcelUseCase(repository, loggedUser, reportRequestRepository, messageBus, unitOfWork);
        }
    }
}
