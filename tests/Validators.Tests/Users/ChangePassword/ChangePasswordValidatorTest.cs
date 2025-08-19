using CashFlow.Application.UseCases.User.ChangePassword;
using CashFlow.Exception;
using CommonTestUtilities.InlineData;
using CommonTestUtilities.Requests;

namespace Validators.Tests.Users.ChangePassword
{
    public class ChangePasswordValidatorTest
    {
        [Fact]
        public void Success()
        {
            // Arrange
            var request = ChangePasswordRequestBuilder.Build();
            var validator = new ChangePasswordValidator();

            // Act
            var result = validator.Validate(request);

            // Assert
            Assert.True(result.IsValid);
        }

        [Theory]
        [ClassData(typeof(InvalidStringInlineData))]
        public void Error_NewPassword_Empty(string newPassword)
        {
            // Arrange
            var request = ChangePasswordRequestBuilder.Build();
            request.NewPassword = newPassword;

            var validator = new ChangePasswordValidator();

            // Act
            var result = validator.Validate(request);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(ResourceErrorMessages.PASSWORD_INVALID, result.Errors.Select(e => e.ErrorMessage));
            Assert.Single(result.Errors);
        }
    }
}
