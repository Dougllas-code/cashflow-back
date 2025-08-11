using CashFlow.Exception;
using CommonTestUtilities.Requests;
using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using WebApi.Tests.InlineData;

namespace WebApi.Tests.Users.Register
{
    public class RegisterUserTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _httpClient;
        private const string METHOD = "api/User";

        public RegisterUserTest(CustomWebApplicationFactory webApplicationFactory)
        {
            _httpClient = webApplicationFactory.CreateClient();
        }

        [Fact]
        public async Task Success()
        {
            //Arrange
            var request = RegisterUserRequestBuilder.Build();

            //Act
            var result = await _httpClient.PostAsJsonAsync(METHOD, request);

            //Assert
            Assert.Equal(System.Net.HttpStatusCode.Created, result.StatusCode);

            var body = await result.Content.ReadAsStreamAsync();
            var response = await JsonDocument.ParseAsync(body);

            var responseName = response.RootElement.GetProperty("name").GetString();
            var token = response.RootElement.GetProperty("token").GetString();

            Assert.NotNull(response);
            Assert.NotNull(responseName);
            Assert.NotEmpty(responseName);
            Assert.Equal(request.Name, responseName);
            Assert.NotNull(token);
            Assert.NotEmpty(token);
        }

        [Theory]
        [ClassData(typeof(CultureInlineData))]
        public async Task Bad_Request(string culture)
        {
            //Arrange
            var request = RegisterUserRequestBuilder.Build();
            request.Name = string.Empty;
            
            _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(culture));

            //Act
            var result = await _httpClient.PostAsJsonAsync(METHOD, request);

            //Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, result.StatusCode);

            var body = await result.Content.ReadAsStreamAsync();
            var response = await JsonDocument.ParseAsync(body);

            var errors = response.RootElement.GetProperty("message").EnumerateArray()
                .Select(e => e.GetString())
                .ToList();

            var expectedErrorMessage = ResourceErrorMessages.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));

            Assert.NotNull(response);
            Assert.True(errors.Count != 0);
            Assert.Single(errors);
            Assert.Equal(expectedErrorMessage, errors.FirstOrDefault());
            Assert.NotEmpty(errors);
        }
    }
}
