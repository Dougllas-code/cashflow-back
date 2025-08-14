using CashFlow.Domain.Enums;
using CashFlow.Exception;
using CommonTestUtilities.InlineData;
using CommonTestUtilities.Requests;
using System.Globalization;
using System.Text.Json;

namespace WebApi.Tests.Expenses.GetById
{
    public class GetExpenseByIdTest: CashFlowClassFixture
    {
        private const string METHOD = "api/Expenses";

        private readonly string _token;
        private readonly long _expenseId;

        public GetExpenseByIdTest(CustomWebApplicationFactory webApplicationFactory):base(webApplicationFactory)
        {
            _token = webApplicationFactory.User_Team_Member.GetToken();
            _expenseId = webApplicationFactory.Expense.GetId();
        }

        [Fact]
        public async Task Success()
        {
            // Act
            var result = await DoGet($"{METHOD}/{_expenseId}", _token);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, result.StatusCode);

            var responseBody = await result.Content.ReadAsStreamAsync();
            var responseData = await JsonDocument.ParseAsync(responseBody);

            Assert.NotNull(responseData);
            Assert.Equal(_expenseId, responseData.RootElement.GetProperty("id").GetInt64());
            Assert.NotNull(responseData.RootElement.GetProperty("title").GetString());
            Assert.NotEmpty(responseData.RootElement.GetProperty("title").GetString()!);
            Assert.True(responseData.RootElement.GetProperty("date").GetDateTime() < DateTime.Today);
            Assert.True(responseData.RootElement.GetProperty("amount").GetDecimal() > 0);

            var paymentTypeValue = responseData.RootElement.GetProperty("paymentType").GetInt32();
            Assert.True(Enum.IsDefined(typeof(PaymentType), paymentTypeValue));
        }

        [Theory]
        [ClassData(typeof(CultureInlineData))]
        public async Task Not_found(string culture)
        {
            // Act
            var result = await DoGet($"{METHOD}/{10000}", _token, culture);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, result.StatusCode);

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
