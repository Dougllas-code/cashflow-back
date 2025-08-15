using System.Text.Json;

namespace WebApi.Tests.Users.GetProfile
{
    public class GetUserProfileTest : CashFlowClassFixture
    {
        private const string METHOD = "api/User";

        private readonly string _token;
        private readonly string _name;
        private readonly string _email;

        public GetUserProfileTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _token = webApplicationFactory.User_Team_Member.GetToken();
            _name = webApplicationFactory.User_Team_Member.GetName();
            _email = webApplicationFactory.User_Team_Member.GetEmail();
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

            var responseName = responseData.RootElement.GetProperty("name").GetString();
            var responseEmail = responseData.RootElement.GetProperty("email").GetString();

            Assert.NotNull(responseData);
            Assert.NotNull(responseName);
            Assert.NotEmpty(responseName);
            Assert.Equal(_name, responseName);
            Assert.NotNull(responseEmail);
            Assert.NotEmpty(responseEmail);
            Assert.Equal(_email, responseEmail);
        }
    }
}
