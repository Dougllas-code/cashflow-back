using CashFlow.Communication.Requests;
using CashFlow.Exception;
using CommonTestUtilities.InlineData;
using CommonTestUtilities.Requests;
using System.Globalization;
using System.Net;
using System.Text.Json;

namespace WebApi.Tests.Login
{
    public class DoLoginTest : CashFlowClassFixture
    {
        private const string METHOD = "api/Login";

        private readonly string _email;
        private readonly string _name;
        private readonly string _password;

        public DoLoginTest(CustomWebApplicationFactory webApplicationFactory): base(webApplicationFactory)
        {
            _email = webApplicationFactory.GetEmail();
            _name = webApplicationFactory.GetName();
            _password = webApplicationFactory.GetPassword();
        }

        [Fact]
        public async Task Success()
        {
            //Arrange
            var request = new LoginRequest
            {
                Email = _email,
                Password = _password
            };

            //Act
            var result = await DoPost(METHOD, request);

            //Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

            var responseBody = await result.Content.ReadAsStreamAsync();
            var responseData = await JsonDocument.ParseAsync(responseBody);

            var responseName = responseData.RootElement.GetProperty("name").GetString();
            var responseToken = responseData.RootElement.GetProperty("token").GetString();

            Assert.NotNull(responseData);
            Assert.NotNull(responseName);
            Assert.NotEmpty(responseName);
            Assert.Equal(_name, responseName);
            Assert.NotNull(responseToken);
            Assert.NotEmpty(responseToken);
        }

        [Theory]
        [ClassData(typeof(CultureInlineData))]
        public async Task Unauthorized(string culture)
        {
            //Arrange
            var request = LoginRequestBuilder.Build();

            //Act
            var result = await DoPost(METHOD, request, culture: culture);

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);

            var responseBody = await result.Content.ReadAsStreamAsync();
            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("message").EnumerateArray()
                .Select(e => e.GetString())
                .ToList();

            var expectedErrorMessage = ResourceErrorMessages.ResourceManager.GetString("EMAIL_OR_PASSWORD_INVALID", new CultureInfo(culture));

            Assert.NotNull(responseData);
            Assert.True(errors.Count != 0);
            Assert.Single(errors);
            Assert.Equal(expectedErrorMessage, errors.FirstOrDefault());
            Assert.NotEmpty(errors);
        }
    }
}
