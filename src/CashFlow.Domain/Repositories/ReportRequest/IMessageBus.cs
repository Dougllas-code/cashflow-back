namespace CashFlow.Domain.Repositories.ReportRequest
{
    public interface IMessageBus
    {
        Task PublishAsync<EventReportRequested>(EventReportRequested message, CancellationToken cancellationToken = default) where EventReportRequested : class;
    }
}
