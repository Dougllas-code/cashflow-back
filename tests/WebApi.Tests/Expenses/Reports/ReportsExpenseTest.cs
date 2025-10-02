using CashFlow.Communication.Enums;
using CashFlow.Communication.Requests;
using CommonTestUtilities.Requests;
using System.Net;
using System.Net.Mime;
using System.Text.Json;

namespace WebApi.Tests.Expenses.Reports
{
    public class ReportsExpenseTest: CashFlowClassFixture 
    {
        private const string METHOD = "api/Report";

        private readonly string _adminToken;
        private readonly string _teamMemberToken;
        private readonly DateTime _expenseDate;

        public ReportsExpenseTest(CustomWebApplicationFactory webApplicationFactory): base(webApplicationFactory)
        {
            _adminToken = webApplicationFactory.User_Admin.GetToken();
            _teamMemberToken = webApplicationFactory.User_Team_Member.GetToken();
            _expenseDate = webApplicationFactory.Expense_Admin.GetDate();
        }

        [Fact]
        public async Task Get_Excel_Success()
        {
            // Arrange
            var request = new ReportRequest
            {
                Month = DateOnly.FromDateTime(_expenseDate),
                ReportType = ReportType.EXCEL
            };

            // Act
            var response = await DoPost($"{METHOD}", request, _adminToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseBody = await response.Content.ReadAsStreamAsync();
            var responseData = await JsonDocument.ParseAsync(responseBody);

            Assert.NotNull(responseData);

            var reportTypeNumber = responseData.RootElement.GetProperty("type").GetInt32();
            var reportTypeEnum = (ReportType)reportTypeNumber;
            Assert.Equal(ReportType.EXCEL, reportTypeEnum);
        }

        [Fact]
        public async Task Get_PDF_Success()
        {
            // Arrange
            var request = new ReportRequest
            {
                Month = DateOnly.FromDateTime(_expenseDate),
                ReportType = ReportType.PDF
            };

            // Act
            var response = await DoPost($"{METHOD}", request, _adminToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseBody = await response.Content.ReadAsStreamAsync();
            var responseData = await JsonDocument.ParseAsync(responseBody);

            Assert.NotNull(responseData);

            var reportTypeNumber = responseData.RootElement.GetProperty("type").GetInt32();
            var reportTypeEnum = (ReportType)reportTypeNumber;
            Assert.Equal(ReportType.PDF, reportTypeEnum);
        }

        [Fact]
        public async Task Error_Forbidden_User_Not_Allowed()
        {
            // Arrange
            var request = new ReportRequest
            {
                Month = DateOnly.FromDateTime(_expenseDate),
                ReportType = ReportType.EXCEL
            };

            // Act
            var response = await DoPost($"{METHOD}", request, _teamMemberToken);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Get_Success_No_Content()
        {
            // Arrange
            var request = new ReportRequest
            {
                Month = DateOnly.FromDateTime(DateTime.Today.Date.AddDays(5)),
                ReportType = ReportType.EXCEL
            };

            // Act
            var response = await DoPost($"{METHOD}", request, _adminToken);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
