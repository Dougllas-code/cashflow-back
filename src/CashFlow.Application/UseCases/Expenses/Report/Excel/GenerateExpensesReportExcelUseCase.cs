using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Repositories.ReportRequest;
using CashFlow.Domain.Services.LoggedUser;

namespace CashFlow.Application.UseCases.Expenses.Report.Excel
{
    public class GenerateExpensesReportExcelUseCase : IGenerateExpensesReportExcelUseCase
    {
        private readonly IExpensesReadOnlyRepository _repository;
        private readonly ILoggedUser _loggedUser;
        private readonly IReportRequestRepository _reportRequestRepository;
        private readonly IMessageBus _messageBus;    
        private readonly IUnitOfWork _unitOfWork;    

        public GenerateExpensesReportExcelUseCase(
            IExpensesReadOnlyRepository repository,
            ILoggedUser loggedUser,
            IReportRequestRepository reportRequestRepository,
            IMessageBus messageBus,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _loggedUser = loggedUser;
            _reportRequestRepository = reportRequestRepository;
            _messageBus = messageBus;
            _unitOfWork = unitOfWork;
        }

        public async Task<ReportRequests?> Execute(DateOnly month)
        {
            var loggedUser = await _loggedUser.Get();

            var expenses = await _repository.GetByMonth(loggedUser, month);

            if (expenses.Count == 0)
            {
                return null;
            }

            var reportRequest = new ReportRequests
            {
                Id = Guid.NewGuid(),
                UserId = loggedUser.Id,
                Status = ReportStatus.PENDING,
                Type = ReportType.EXCEL,
                RequestDate = DateTime.UtcNow,
                Month = month
            };

            await _reportRequestRepository.Create(reportRequest);

            var eventReportRequested = new EventReportRequested
            {
                Id = reportRequest.Id, 
                UserId = loggedUser.Id, 
                Month = reportRequest.Month,
                ReportType = reportRequest.Type
            };

            await _messageBus.PublishAsync(eventReportRequested);

            await _unitOfWork.Commit();

            return reportRequest;
        }
    }

}
