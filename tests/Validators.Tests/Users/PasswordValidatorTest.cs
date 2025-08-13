using CashFlow.Application.SharedValidators;
using CashFlow.Communication.Requests;
using CommonTestUtilities.InlineData;
using FluentValidation;

namespace Validators.Tests.Users
{
    public class PasswordValidatorTest
    {
        [Theory]
        [ClassData(typeof(InvalidPasswordInlineData))]
        public void Error_Password_Invalid(string password)
        {
            //Arrange
            var validator = new PasswordValidator<UserRequest>();

            //Act
            var result = validator.IsValid(new ValidationContext<UserRequest>(new UserRequest()), password);

            //Assert
            Assert.False(result);
        }

    }
}
