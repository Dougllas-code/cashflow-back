using CashFlow.Application.UseCases.User.Update;
using CashFlow.Exception;
using CommonTestUtilities.Entities.User;
using CommonTestUtilities.InlineData;
using CommonTestUtilities.Requests;

namespace Validators.Tests.Users.Update
{
    public class UdpateUserValidatorTests
    {
        [Fact]
        public void Success()
        {
            // Arrange
            var request = UpdateUserRequestBuilder.Build();

            var validator = new UpdateUserValidator();

            // Act
            var result = validator.Validate(request);

            // Assert
            Assert.True(result.IsValid);
        }

        [Theory]
        [ClassData(typeof(InvalidStringInlineData))]
        public void Error_Name_Invalid(string name)
        {
            //Arrange
            var request = UpdateUserRequestBuilder.Build();
            request.Name = name;

            var validator = new UpdateUserValidator();

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
            var request = UpdateUserRequestBuilder.Build();
            request.Email = email;

            var validator = new UpdateUserValidator();

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
            var request = UpdateUserRequestBuilder.Build();
            request.Email = "jhondoe.com";

            var validator = new UpdateUserValidator();

            //Act
            var result = validator.Validate(request);

            //Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(request.Email) && e.ErrorMessage == ResourceErrorMessages.EMAIL_INVALID);
        }
    }
}
