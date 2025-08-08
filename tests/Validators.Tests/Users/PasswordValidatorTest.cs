using CashFlow.Application.SharedValidators;
using CashFlow.Communication.Requests;
using FluentValidation;

namespace Validators.Tests.Users
{
    public class PasswordValidatorTest
    {
        [Theory]
        [InlineData("")]
        [InlineData("      ")]
        [InlineData(null)]
        [InlineData("a")]
        [InlineData("aa")]
        [InlineData("aaa")]
        [InlineData("aaaa")]
        [InlineData("aaaaa")]
        [InlineData("aaaaaa")]
        [InlineData("aaaaaaa")]
        [InlineData("aaaaaaaa")]
        [InlineData("AAAAAAAA")]
        [InlineData("Aaaaaaaa")]
        [InlineData("Aaaaaaa1")]
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
