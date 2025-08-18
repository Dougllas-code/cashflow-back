using CashFlow.Application.SharedValidators;
using CashFlow.Communication.Requests;
using FluentValidation;

namespace CashFlow.Application.UseCases.User.ChangePassword
{
    public class ChangePasswordValidator: AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordValidator()
        {
            RuleFor(request => request.NewPassword).SetValidator(new PasswordValidator<ChangePasswordRequest>());
        }
    }
}
