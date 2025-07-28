using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses.User;

namespace CashFlow.Application.UseCases.User.Register
{
    public interface IRegisterUserUseCase
    {
        Task<RegisterUserResponse> Execute(UserRequest request);
    }
}
