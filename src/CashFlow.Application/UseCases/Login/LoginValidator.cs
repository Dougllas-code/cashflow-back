using CashFlow.Application.SharedValidators;
using CashFlow.Communication.Requests;
using CashFlow.Exception;
using FluentValidation;

namespace CashFlow.Application.UseCases.Login
{
    public class LoginValidator : AbstractValidator<LoginRequest>
    {
        public LoginValidator()
        {
            RuleFor(login => login.Email)
                .NotEmpty()
                .WithMessage(ResourceErrorMessages.EMAIL_EMPTY)
                .EmailAddress()
                .When(user => !string.IsNullOrWhiteSpace(user.Email), ApplyConditionTo.CurrentValidator)
                .WithMessage(ResourceErrorMessages.EMAIL_INVALID);
            RuleFor(user => user.Password).SetValidator(new PasswordValidator<LoginRequest>());
        }
    }
}
