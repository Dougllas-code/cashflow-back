using CashFlow.Application.UseCases.Expenses.Report;
using CashFlow.Communication.Enums;
using CashFlow.Communication.Requests;
using CashFlow.Domain.Entities;
using CommonTestUtilities.Entities.User;
using CommonTestUtilities.LoggerUser;
using CommonTestUtilities.Repositories;

namespace UseCases.Tests.Expenses.Reports
{
    public class GenerateExpenseReportUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            // Arrange
            var user = UserBuilder.Build();
            var expenses = ExpenseBuilder.Collection(user);
            var request = new ReportRequest
            {
                Month = DateOnly.FromDateTime(DateTime.Today),
                ReportType = ReportType.EXCEL
            };

            var useCase = CreateUseCase(user, expenses);

            // Act
            var result = await useCase.Execute(request);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Success_Empty()
        {
            // Arrange
            var user = UserBuilder.Build();
            var request = new ReportRequest
            {
                Month = DateOnly.FromDateTime(DateTime.Today),
                ReportType = ReportType.EXCEL
            };

            var useCase = CreateUseCase(user, []);

            // Act
            var result = await useCase.Execute(request);

            // Assert
            Assert.Null(result);
        }

        private static GenerateExpensesReportUseCase CreateUseCase(User user, List<Expense> expenses)
        {
            var repository = new ExpensesReadOnlyRepositoryBuilder().GetByMonth(user, expenses).Build();
            var loggedUser = LoggedUserBuilder.Build(user);
            var reportRequestRepository = ReportRequestRepositoryBuilder.Build();
            var messageBus = MessageBusBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();

            return new GenerateExpensesReportUseCase(repository, loggedUser, reportRequestRepository, messageBus, unitOfWork);
        }
    }
}
