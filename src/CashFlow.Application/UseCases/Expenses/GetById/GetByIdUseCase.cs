using AutoMapper;
using CashFlow.Communication.Responses.Expenses;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception;
using CashFlow.Exception.BaseExceptions;

namespace CashFlow.Application.UseCases.Expenses.GetById
{
    public class GetByIdUseCase : IGetByIdUseCase
    {
        private readonly IExpensesReadOnlyRepository _repository;
        private readonly IMapper _mapper;

        public GetByIdUseCase(
            IExpensesReadOnlyRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ExpenseResponse> Execute(long id)
        {
            var result = await _repository.GetById(id);

            if(result is null)
            {
                throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
            }

            return _mapper.Map<ExpenseResponse>(result);
        }
    }
}
