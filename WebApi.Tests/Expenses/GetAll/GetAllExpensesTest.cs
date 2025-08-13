using System.Text.Json;

namespace WebApi.Tests.Expenses.GetAll
{
    public class GetAllExpensesTest : CashFlowClassFixture
    {
        private const string METHOD = "api/Expenses";

        private readonly string _token;

        public GetAllExpensesTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _token = webApplicationFactory.GetToken();
        }

        [Fact]
        public async Task Success()
        {
            // Act
            var result = await DoGet(METHOD, _token);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, result.StatusCode);

            var responseBody = await result.Content.ReadAsStreamAsync();
            var responseData = await JsonDocument.ParseAsync(responseBody);

            Assert.NotNull(responseData);
            Assert.True(responseData.RootElement.GetProperty("expenses").GetArrayLength() > 0);
            foreach (var expense in responseData.RootElement.GetProperty("expenses").EnumerateArray())
            {
                Assert.NotEqual(0, expense.GetProperty("id").GetInt64());
                Assert.NotNull(expense.GetProperty("title").GetString());
                Assert.True(expense.GetProperty("amount").GetDecimal() > 0);
            }
        }
    }
}
