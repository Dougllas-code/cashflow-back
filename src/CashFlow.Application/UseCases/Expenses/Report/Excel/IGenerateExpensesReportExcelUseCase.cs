using CashFlow.Domain.Entities;

namespace CashFlow.Application.UseCases.Expenses.Report.Excel
{
    public interface IGenerateExpensesReportExcelUseCase
    {
        Task<ReportRequests?> Execute(DateOnly month);
    }
}
