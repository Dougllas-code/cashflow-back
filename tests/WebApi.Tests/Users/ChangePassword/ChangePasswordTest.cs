using CashFlow.Communication.Requests;
using CashFlow.Exception;
using CommonTestUtilities.InlineData;
using CommonTestUtilities.Requests;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace WebApi.Tests.Users.ChangePassword
{
    public class ChangePasswordTest:CashFlowClassFixture
    {
        private const string METHOD = "/api/User/change-password";

        private readonly string _token;
        private readonly string _password;
        private readonly string _email;

        public ChangePasswordTest(CustomWebApplicationFactory webApplicationFactory):base(webApplicationFactory)
        {
            _token = webApplicationFactory.User_Team_Member.GetToken();
            _password = webApplicationFactory.User_Team_Member.GetPassword();
            _email = webApplicationFactory.User_Team_Member.GetEmail();
        }

        [Fact]
        public async Task Success()
        {
            //Arrange
            var request = ChangePasswordRequestBuilder.Build();
            request.CurrentPassword = _password;

            //Act
            var result = await DoPut(METHOD, request, _token);

            //Assert
            Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);

            var loginRequest = new LoginRequest
            {
                Email = _email,
                Password = _password
            };

            result = await DoPost("/api/login", loginRequest);
            Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);

            loginRequest.Password = request.NewPassword;

            result = await DoPost("/api/login", loginRequest);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Theory]
        [ClassData(typeof(CultureInlineData))]
        public async Task Error_Current_Password_Different(string culture)
        {
            //Arrange
            var request = ChangePasswordRequestBuilder.Build();

            //Act
            var result = await DoPut(METHOD, request, _token, culture);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

            var responseBody = await result.Content.ReadAsStreamAsync();
            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("message").EnumerateArray()
                .Select(e => e.GetString())
                .ToList();

            var expectedErrorMessage = ResourceErrorMessages.ResourceManager.GetString("CURRENT_PASSWORD_INCORRECT", new CultureInfo(culture));

            Assert.NotNull(responseData);
            Assert.NotEmpty(errors);
            Assert.Single(errors);
            Assert.Equal(expectedErrorMessage, errors.FirstOrDefault());
        }



        
    }
}
