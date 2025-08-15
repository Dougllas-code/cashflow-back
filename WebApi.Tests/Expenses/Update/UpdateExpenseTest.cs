using CashFlow.Exception;
using CommonTestUtilities.InlineData;
using CommonTestUtilities.Requests;
using System.Globalization;
using System.Net;
using System.Text.Json;

namespace WebApi.Tests.Expenses.Update
{
    public class UpdateExpenseTest: CashFlowClassFixture
    {
        private const string METHOD = "api/Expenses";

        private readonly string _token;
        private readonly long _expenseId;

        public UpdateExpenseTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _token = webApplicationFactory.User_Team_Member.GetToken();
            _expenseId = webApplicationFactory.Expense.GetId();
        }

        [Fact]
        public async Task Success()
        {
            // Arrange
            var request = ExpenseRequestBuilder.Build();

            // Act
            var putResult = await DoPut($"{METHOD}/{_expenseId}", request, _token);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, putResult.StatusCode);

            // Verify that the expense is updated
            var getResult = await DoGet($"{METHOD}/{_expenseId}", _token);

            Assert.Equal(HttpStatusCode.OK, getResult.StatusCode);

            var responseBody = await getResult.Content.ReadAsStreamAsync();
            var responseData = await JsonDocument.ParseAsync(responseBody);

            var title = responseData.RootElement.GetProperty("title").GetString();
            var description = responseData.RootElement.GetProperty("description").GetString();
            var date = responseData.RootElement.GetProperty("date").GetDateTime();
            var amount = responseData.RootElement.GetProperty("amount").GetDecimal();
            var paymentType = responseData.RootElement.GetProperty("paymentType").GetInt32();

            Assert.NotNull(responseData);
            Assert.Equal(request.Title, title);
            Assert.Equal(request.Description, description);
            Assert.Equal(request.Date, date);
            Assert.Equal(request.Amount, amount);
            Assert.Equal((int)request.PaymentType, paymentType);
        }

        [Theory]
        [ClassData(typeof(CultureInlineData))]
        public async Task Bad_Request(string culture)
        {
            // Arrange
            var request = ExpenseRequestBuilder.Build();
            request.Title = string.Empty;

            // Act
            var result = await DoPut($"{METHOD}/{_expenseId}", request, _token, culture);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

            var responseBody = await result.Content.ReadAsStreamAsync();
            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("message").EnumerateArray()
               .Select(e => e.GetString())
               .ToList();

            var expectedErrorMessage = ResourceErrorMessages.ResourceManager.GetString("TITLE_REQUIRED", new CultureInfo(culture));

            Assert.NotNull(responseData);
            Assert.NotEmpty(errors);
            Assert.Single(errors);
            Assert.Equal(expectedErrorMessage, errors.FirstOrDefault());
        }

        [Theory]
        [ClassData(typeof(CultureInlineData))]
        public async Task Not_Found(string culture)
        {
            // Arrange
            var request = ExpenseRequestBuilder.Build();

            // Act
            var result = await DoPut($"{METHOD}/99999999", request, _token, culture);

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
