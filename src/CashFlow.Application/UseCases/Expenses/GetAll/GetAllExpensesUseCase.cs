using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Expenses;

namespace CashFlow.Application.UseCases.Expenses.GetAll
{
    public class GetAllExpensesUseCase : IGetAllExpensesUseCase
    {
        private readonly IExpensesReadOnlyRepository _expenseRepository;
        private readonly IMapper _mapper;

        public GetAllExpensesUseCase(
            IExpensesReadOnlyRepository expenseRepository,
            IMapper mapper)
        {
            _expenseRepository = expenseRepository;
            _mapper = mapper;
        }

        public async Task<ExpensesResponse> Execute()
        {
            var result = await _expenseRepository.GetAll();
            return new ExpensesResponse {
                Expenses = _mapper.Map<List<ExpenseShortResponse>>(result)
            };
        }
    }
}
