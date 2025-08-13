using CashFlow.Exception;
using CommonTestUtilities.InlineData;
using CommonTestUtilities.Requests;
using System.Globalization;
using System.Text.Json;

namespace WebApi.Tests.Users.Register
{
    public class RegisterUserTest : CashFlowClassFixture
    {
        private const string METHOD = "api/User";

        public RegisterUserTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory) { }

        [Fact]
        public async Task Success()
        {
            //Arrange
            var request = RegisterUserRequestBuilder.Build();

            //Act
            var result = await DoPost(METHOD, request);

            //Assert
            Assert.Equal(System.Net.HttpStatusCode.Created, result.StatusCode);

            var responseBody = await result.Content.ReadAsStreamAsync();
            var responseData = await JsonDocument.ParseAsync(responseBody);

            var responseName = responseData.RootElement.GetProperty("name").GetString();
            var token = responseData.RootElement.GetProperty("token").GetString();

            Assert.NotNull(responseData);
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

            //Act
            var result = await DoPost(METHOD, request, culture: culture);

            //Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, result.StatusCode);

            var responseBody = await result.Content.ReadAsStreamAsync();
            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("message").EnumerateArray()
                .Select(e => e.GetString())
                .ToList();

            var expectedErrorMessage = ResourceErrorMessages.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));

            Assert.NotNull(responseData);
            Assert.NotEmpty(errors);
            Assert.Single(errors);
            Assert.Equal(expectedErrorMessage, errors.FirstOrDefault());
        }
    }
}
