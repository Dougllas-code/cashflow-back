using System.Net;
using System.Net.Mime;
using System.Text.Json;
using static MassTransit.ValidationResultExtensions;

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
            var month = DateOnly.FromDateTime(_expenseDate);

            // Act
            var response = await DoGet($"{METHOD}/excel?month={month:yyyy-MM-dd}", _adminToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseBody = await response.Content.ReadAsStreamAsync();
            var responseData = await JsonDocument.ParseAsync(responseBody);

            Assert.NotNull(responseData);
        }

        [Fact]
        public async Task Get_PDF_Success()
        {
            // Arrange
            var month = DateOnly.FromDateTime(_expenseDate);

            // Act
            var response = await DoGet($"{METHOD}/pdf?month={month:yyyy-MM-dd}", _adminToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response.Content.Headers.ContentType);
            Assert.Equal(MediaTypeNames.Application.Pdf, response.Content.Headers.ContentType!.MediaType);
        }

        [Fact]
        public async Task Error_Forbidden_User_Not_Allowed_Excel()
        {
            // Arrange
            var month = DateOnly.FromDateTime(_expenseDate);

            // Act
            var response = await DoGet($"{METHOD}/excel?month={month:yyyy-MM-dd}", _teamMemberToken);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Error_Forbidden_User_Not_Allowed_PDF()
        {
            // Arrange
            var month = DateOnly.FromDateTime(_expenseDate);

            // Act
            var response = await DoGet($"{METHOD}/pdf?month={month:yyyy-MM-dd}", _teamMemberToken);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Get_Excel_Success_No_Content()
        {
            // Arrange
            var month = DateOnly.FromDateTime(DateTime.Today.Date.AddDays(5));

            // Act
            var response = await DoGet($"{METHOD}/excel?month={month:yyyy-MM-dd}", _adminToken);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task Get_PDF_Success_No_Content()
        {
            // Arrange
            var month = DateOnly.FromDateTime(DateTime.Today.Date.AddDays(5));

            // Act
            var response = await DoGet($"{METHOD}/pdf?month={month:yyyy-MM-dd}", _adminToken);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
