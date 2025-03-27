namespace CashFlow.Application.UseCases.Expenses.Report.PDF
{
    public interface IGenerateExpensesReportPdfUseCase
    {
        Task<byte[]> Execute(DateOnly month);
    }
}
