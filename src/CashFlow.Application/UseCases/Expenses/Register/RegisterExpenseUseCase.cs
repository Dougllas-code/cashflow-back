using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception.BaseExceptions;

namespace CashFlow.Application.UseCases.Expenses.Register
{
    internal class RegisterExpenseUseCase : IRegisterExpenseUseCase
    {

        private readonly IExpensesWriteOnlyRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RegisterExpenseUseCase(
            IExpensesWriteOnlyRepository repository, 
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<RegisterExpenseResponse> Execute(ExpenseRequest request)
        {
            ValidateRequest(request);

            var entity = _mapper.Map<Expense>(request);

            await _repository.Add(entity);
            await _unitOfWork.Commit();

            return _mapper.Map<RegisterExpenseResponse>(entity);
        }

        private static void ValidateRequest(ExpenseRequest request)
        {
            var result = new ExpenseValidator().Validate(request);
            if (!result.IsValid)
            {
                    var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
                    throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
