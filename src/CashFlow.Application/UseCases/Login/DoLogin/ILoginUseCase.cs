using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses.User;

namespace CashFlow.Application.UseCases.Login.DoLogin
{
    public interface ILoginUseCase
    {
        Task<RegisterUserResponse> Execute(LoginRequest request);
    }
}
