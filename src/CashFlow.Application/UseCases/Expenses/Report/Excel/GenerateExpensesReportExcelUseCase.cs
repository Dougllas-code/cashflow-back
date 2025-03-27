using CashFlow.Domain.Extensions;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Resources.Report;
using ClosedXML.Excel;

namespace CashFlow.Application.UseCases.Expenses.Report.Excel
{
    public class GenerateExpensesReportExcelUseCase : IGenerateExpensesReportExcelUseCase
    {
        private const string CURRENCY_SYMBOL = "€";
        private readonly IExpensesReadOnlyRepository _repository;

        public GenerateExpensesReportExcelUseCase(IExpensesReadOnlyRepository repository)
        {
            _repository = repository;
        }

        public async Task<byte[]> Execute(DateOnly month)
        {
            var expenses = await _repository.GetByMonth(month);
            if (expenses.Count == 0)
            {
                return [];
            }

            using var workbook = new XLWorkbook();

            workbook.Author = "CashFlow Report Excel";

            var worksheet = workbook.Worksheets.Add(month.ToString("Y"));
            InsertHeader(worksheet);

            var row = 2;
            foreach (var expense in expenses) 
            {
                worksheet.Cell($"A{row}").Value = expense.Title;
                worksheet.Cell($"B{row}").Value = expense.Date;
                worksheet.Cell($"C{row}").Value = expense.PaymentType.PaymentTypeToString();

                worksheet.Cell($"D{row}").Value = expense.Amount;
                worksheet.Cell($"D{row}").Style.NumberFormat.Format = $"-{CURRENCY_SYMBOL} #,##0.00";

                worksheet.Cell($"E{row}").Value = expense.Description;

                row++;
            }

            worksheet.Columns().AdjustToContents();

            var file = new MemoryStream();
            workbook.SaveAs(file);

            return file.ToArray();
        }

        private static void InsertHeader(IXLWorksheet worksheet)
        {
            worksheet.Cell("A1").Value = ResourceReportGenerationMessages.TITLE;
            worksheet.Cell("B1").Value = ResourceReportGenerationMessages.DATE;
            worksheet.Cell("C1").Value = ResourceReportGenerationMessages.PAYMENT_TYPE;
            worksheet.Cell("D1").Value = ResourceReportGenerationMessages.AMOUNT;
            worksheet.Cell("E1").Value = ResourceReportGenerationMessages.DESCRIPTION;

            worksheet.Cells("A1:E1").Style.Font.Bold = true;
            worksheet.Cells("A1:E1").Style.Fill.BackgroundColor = XLColor.LightGray;

            worksheet.Cell("A1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Cell("B1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Cell("C1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Cell("E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            worksheet.Cell("D1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
        }
    }

}
