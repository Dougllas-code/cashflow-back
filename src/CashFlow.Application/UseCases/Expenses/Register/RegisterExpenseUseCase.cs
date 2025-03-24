using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception.BaseExceptions;

namespace CashFlow.Application.UseCases.Expenses.Register
{
    public class RegisterExpenseUseCase : IRegisterExpenseUseCase
    {

        private readonly IExpensesWriteOnlyRepository _expenseRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RegisterExpenseUseCase(
            IExpensesWriteOnlyRepository expenseRepository, 
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _expenseRepository = expenseRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<RegisterExpenseResponse> Execute(RegisterExpenseRequest request)
        {
            ValidateRequest(request);

            var entity = _mapper.Map<Expense>(request);

            await _expenseRepository.Add(entity);
            await _unitOfWork.Commit();

            return _mapper.Map<RegisterExpenseResponse>(entity);
        }

        private static void ValidateRequest(RegisterExpenseRequest request)
        {
            var result = new RegisterExpenseValidator().Validate(request);
            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
