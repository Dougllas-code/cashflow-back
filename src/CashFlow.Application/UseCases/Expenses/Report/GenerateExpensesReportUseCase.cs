using CashFlow.Communication.Requests;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Repositories.ReportRequest;
using CashFlow.Domain.Services.LoggedUser;

namespace CashFlow.Application.UseCases.Expenses.Report
{
    public class GenerateExpensesReportUseCase : IGenerateExpensesReportUseCase
    {
        private readonly IExpensesReadOnlyRepository _repository;
        private readonly ILoggedUser _loggedUser;
        private readonly IReportRequestRepository _reportRequestRepository;
        private readonly IMessageBus _messageBus;    
        private readonly IUnitOfWork _unitOfWork;    

        public GenerateExpensesReportUseCase(
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

        public async Task<ReportRequests?> Execute(ReportRequest request)
        {
            var loggedUser = await _loggedUser.Get();

            var expenses = await _repository.GetByMonth(loggedUser, request.Month);

            if (expenses.Count == 0)
            {
                return null;
            }

            var reportRequest = new ReportRequests
            {
                Id = Guid.NewGuid(),
                UserId = loggedUser.Id,
                Status = ReportStatus.PENDING,
                Type = (ReportType)request.ReportType,
                RequestDate = DateTime.UtcNow,
                Month = request.Month
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
