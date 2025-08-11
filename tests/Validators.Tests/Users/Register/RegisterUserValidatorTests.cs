using CashFlow.Application.UseCases.User;
using CashFlow.Exception;
using CommonTestUtilities.InlineData;
using CommonTestUtilities.Requests;

namespace Validators.Tests.Users.Register
{
    public class RegisterUserValidatorTests
    {
        [Fact]
        public void Success()
        {
            //Arrange
            var validator = new UserValidator();
            var request = RegisterUserRequestBuilder.Build();

            //Act
            var result = validator.Validate(request);

            //Assert
            Assert.True(result.IsValid);
        }

        [Theory]
        [ClassData(typeof(InvalidStringInlineData))]
        public void Error_Name_Invalid(string name)
        {
            //Arrange
            var validator = new UserValidator();
            var request = RegisterUserRequestBuilder.Build();
            request.Name = name;

            //Act
            var result = validator.Validate(request);

            //Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(request.Name) && e.ErrorMessage == ResourceErrorMessages.NAME_EMPTY);
        }

        [Theory]
        [ClassData(typeof(InvalidStringInlineData))]
        public void Error_Email_Empty(string email)
        {
            //Arrange
            var validator = new UserValidator();
            var request = RegisterUserRequestBuilder.Build();
            request.Email = email;

            //Act
            var result = validator.Validate(request);

            //Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(request.Email) && e.ErrorMessage == ResourceErrorMessages.EMAIL_EMPTY);
        }

        [Fact]
        public void Error_Email_Invalid()
        {
            //Arrange
            var validator = new UserValidator();
            var request = RegisterUserRequestBuilder.Build();
            request.Email = "jhondoe.com";

            //Act
            var result = validator.Validate(request);

            //Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(request.Email) && e.ErrorMessage == ResourceErrorMessages.EMAIL_INVALID);
        }

        [Fact]
        public void Error_Password_Empty()
        {
            //Arrange
            var validator = new UserValidator();
            var request = RegisterUserRequestBuilder.Build();
            request.Password = string.Empty;

            //Act
            var result = validator.Validate(request);

            //Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(request.Password) && e.ErrorMessage == ResourceErrorMessages.PASSWORD_INVALID);
        }


    }
}
