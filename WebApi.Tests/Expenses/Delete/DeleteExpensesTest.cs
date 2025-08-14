using CashFlow.Exception;
using CommonTestUtilities.InlineData;
using System.Globalization;
using System.Net;
using System.Text.Json;

namespace WebApi.Tests.Expenses.Delete
{
    public class DeleteExpensesTest: CashFlowClassFixture
    {
        private const string METHOD = "api/Expenses";

        private readonly string _token;
        private readonly long _expenseId;

        public DeleteExpensesTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _token = webApplicationFactory.User_Team_Member.GetToken();
            _expenseId = webApplicationFactory.Expense.GetId();
        }

        [Fact]
        public async Task Success()
        {
            // Act
            var result = await DoDelete($"{METHOD}/{_expenseId}", _token);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);

            // Verify that the expense is deleted
            var getResult = await DoGet($"{METHOD}/{_expenseId}", _token);
            Assert.Equal(HttpStatusCode.NotFound, getResult.StatusCode);
        }

        [Theory]
        [ClassData(typeof(CultureInlineData))]
        public async Task NotFound(string culture)
        {
            // Act
            var result = await DoDelete($"{METHOD}/999999", _token, culture);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);

            var responseBody = await result.Content.ReadAsStreamAsync();
            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("message").EnumerateArray()
               .Select(e => e.GetString())
               .ToList();

            var expectedErrorMessage = ResourceErrorMessages.ResourceManager.GetString("EXPENSE_NOT_FOUND", new CultureInfo(culture));

            Assert.NotNull(responseData);
            Assert.NotEmpty(errors);
            Assert.Single(errors);
            Assert.Equal(expectedErrorMessage, errors.FirstOrDefault());
        }
    }
}
