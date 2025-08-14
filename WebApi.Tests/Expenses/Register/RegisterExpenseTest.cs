using CashFlow.Exception;
using CommonTestUtilities.InlineData;
using CommonTestUtilities.Requests;
using System.Globalization;
using System.Text.Json;

namespace WebApi.Tests.Expenses.Register
{
    public class RegisterExpenseTest: CashFlowClassFixture
    {
        private const string METHOD = "api/Expenses";

        private readonly string _token;

        public RegisterExpenseTest(CustomWebApplicationFactory webApplicationFactory): base(webApplicationFactory)
        {
            _token = webApplicationFactory.User_Team_Member.GetToken();
        }

        [Fact]
        public async Task Success()
        {
            // Arrange
            var request = RegisterExpenseRequestBuilder.Build();

            // Act
            var result = await DoPost(METHOD, request, _token);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.Created, result.StatusCode);

            var responseBody = await result.Content.ReadAsStreamAsync();
            var responseData = await JsonDocument.ParseAsync(responseBody);

            var title = responseData.RootElement.GetProperty("title").GetString();

            Assert.NotNull(responseData);
            Assert.NotNull(title);
            Assert.NotEmpty(title);
            Assert.Equal(request.Title, title);
        }

        [Theory]
        [ClassData(typeof(CultureInlineData))]
        public async Task Bad_Request(string culture)
        {
            // Arrange
            var request = RegisterExpenseRequestBuilder.Build();
            request.Title = string.Empty;

            // Act
            var result = await DoPost(METHOD, request, _token, culture);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, result.StatusCode);

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
    }
}
