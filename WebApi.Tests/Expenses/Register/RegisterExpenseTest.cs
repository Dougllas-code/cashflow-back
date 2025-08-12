using CashFlow.Exception;
using CommonTestUtilities.InlineData;
using CommonTestUtilities.Requests;
using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace WebApi.Tests.Expenses.Register
{
    public class RegisterExpenseTest: IClassFixture<CustomWebApplicationFactory>
    {
        private const string METHOD = "api/Expenses";

        private readonly HttpClient _httpClient;
        private readonly string _token;

        public RegisterExpenseTest(CustomWebApplicationFactory webApplicationFactory)
        {
            _httpClient = webApplicationFactory.CreateClient();
            _token = webApplicationFactory.GetToken();
        }

        [Fact]
        public async Task Success()
        {
            // Arrange
            var request = RegisterExpenseRequestBuilder.Build();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            // Act
            var result = await _httpClient.PostAsJsonAsync(METHOD, request);

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

            _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(culture));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            // Act
            var result = await _httpClient.PostAsJsonAsync(METHOD, request);

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
