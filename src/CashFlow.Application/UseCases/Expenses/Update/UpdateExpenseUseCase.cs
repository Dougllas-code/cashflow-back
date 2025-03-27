using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception;
using CashFlow.Exception.BaseExceptions;

namespace CashFlow.Application.UseCases.Expenses.Update
{
    public class UpdateExpenseUseCase : IUpdateExpenseUseCase
    {
        private readonly IUpdateOnlyExpenseRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateExpenseUseCase(
            IUpdateOnlyExpenseRepository repository,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(long id, ExpenseRequest request)
        {
            ValidateRequest(request);

            var expense = await _repository.GetById(id);

            if (expense is null)
            {
                throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
            }

            var result = _mapper.Map(source: request, destination: expense);
            expense.Id = id;

            _repository.Update(result);
            await _unitOfWork.Commit();
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
