using AutoMapper;
using CashFlow.Communication.Responses.Expenses;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.BaseExceptions;

namespace CashFlow.Application.UseCases.Expenses.GetById
{
    public class GetByIdUseCase : IGetByIdUseCase
    {
        private readonly IExpensesReadOnlyRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILoggedUser _loggedUser;

        public GetByIdUseCase(
            IExpensesReadOnlyRepository repository,
            IMapper mapper,
            ILoggedUser loggedUser)
        {
            _repository = repository;
            _mapper = mapper;
            _loggedUser = loggedUser;
        }

        public async Task<ExpenseResponse> Execute(long id)
        {
            var loggedUser = await _loggedUser.Get();
            var result = await _repository.GetById(loggedUser, id);

            if(result is null)
            {
                throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
            }

            return _mapper.Map<ExpenseResponse>(result);
        }
    }
}
