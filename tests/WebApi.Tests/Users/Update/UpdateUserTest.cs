using CashFlow.Exception;
using CommonTestUtilities.InlineData;
using CommonTestUtilities.Requests;
using System.Globalization;
using System.Text.Json;

namespace WebApi.Tests.Users.Update
{
    public class UpdateUserTest: CashFlowClassFixture
    {
        private const string METHOD = "api/User";

        private readonly string _token;

        public UpdateUserTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _token = webApplicationFactory.User_Team_Member.GetToken();
        }

        [Fact]
        public async Task Success()
        {
            // Arrange
            var request = UpdateUserRequestBuilder.Build();

            // Act
            var result = await DoPut(METHOD, request, token: _token);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NoContent, result.StatusCode);
        }

        [Theory]
        [ClassData(typeof(CultureInlineData))]
        public async Task Error_Name_Empty(string culture)
        {
            // Arrange
            var request = UpdateUserRequestBuilder.Build();
            request.Name = string.Empty;

            // Act
            var result = await DoPut(METHOD, request, token: _token, culture);

            // Assert
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
