using System.Net;

namespace WebApi.Tests.Users.Delete
{
    public class DeleteUserTest: CashFlowClassFixture
    {

        private const string METHOD = "api/User";

        private readonly string _token;

        public DeleteUserTest(CustomWebApplicationFactory webApplicationFactory): base(webApplicationFactory)
        {
            _token = webApplicationFactory.User_Team_Member.GetToken();
        }

        [Fact]
        public async Task Success()
        {
            // Act
            var response = await DoDelete(METHOD, _token);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
