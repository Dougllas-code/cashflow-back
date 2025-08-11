using CashFlow.Application.UseCases.Expenses;
using CashFlow.Communication.Enums;
using CashFlow.Exception;
using CommonTestUtilities.InlineData;
using CommonTestUtilities.Requests;

namespace Validators.Tests.Expenses.Register
{
    public class RegisterExpenseValidatorTests
    {
        [Fact]
        public void Success()
        {
            //Arrange
            var validator = new ExpenseValidator();
            var request = RegisterExpenseRequestBuilder.Build();

            //Act
            var result = validator.Validate(request);

            //Assert
            Assert.True(result.IsValid);
        }

        [Theory]
        [ClassData(typeof(InvalidStringInlineData))]
        public void ErrorTitleEmpty(string title)
        {
            //Arrange
            var validator = new ExpenseValidator();
            var request = RegisterExpenseRequestBuilder.Build();
            request.Title = title;

            //Act
            var result = validator.Validate(request);

            //Assert
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Contains(result.Errors, error => error.ErrorMessage.Equals(ResourceErrorMessages.TITLE_REQUIRED));
        }

        [Fact]
        public void ErrorDateFuture()
        {
            //Arrange
            var validator = new ExpenseValidator();
            var request = RegisterExpenseRequestBuilder.Build();
            request.Date = DateTime.UtcNow.AddDays(1);

            //Act
            var result = validator.Validate(request);

            //Assert
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Contains(result.Errors, error => error.ErrorMessage.Equals(ResourceErrorMessages.EXPENSES_CANNOT_BE_FOR_THE_FUTURE));
        }

        [Fact]
        public void ErrorPaymentTypeInvalid()
        {
            //Arrange
            var validator = new ExpenseValidator();
            var request = RegisterExpenseRequestBuilder.Build();
            request.PaymentType = (PaymentType)700;

            //Act
            var result = validator.Validate(request);

            //Assert
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Contains(result.Errors, error => error.ErrorMessage.Equals(ResourceErrorMessages.PAYMENT_TYPE_INVALID));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void ErrorAmountInvalid(decimal amount)
        {
            //Arrange
            var validator = new ExpenseValidator();
            var request = RegisterExpenseRequestBuilder.Build();
            request.Amount = amount;

            //Act
            var result = validator.Validate(request);

            //Assert
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Contains(result.Errors, error => error.ErrorMessage.Equals(ResourceErrorMessages.AMOUNT_MUST_BE_GREATER_THAN_ZERO));
        }
    }
}
