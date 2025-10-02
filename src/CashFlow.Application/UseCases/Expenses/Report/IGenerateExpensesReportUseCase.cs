using CashFlow.Communication.Requests;
using CashFlow.Domain.Entities;

namespace CashFlow.Application.UseCases.Expenses.Report
{
    public interface IGenerateExpensesReportUseCase
    {
        Task<ReportRequests?> Execute(ReportRequest request);
    }
}
