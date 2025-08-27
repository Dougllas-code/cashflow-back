using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCases.CheckToken
{
    public interface ICheckTokenUseCase
    {
        CheckTokenResponse Execute();
    }
}
