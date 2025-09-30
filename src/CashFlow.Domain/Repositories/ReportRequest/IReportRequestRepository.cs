using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.ReportRequest
{
    public interface IReportRequestRepository
    {
        Task Create(ReportRequests reportRequest);
    }
}
