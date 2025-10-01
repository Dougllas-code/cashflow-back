using CashFlow.Domain.Repositories.ReportRequest;

namespace CommonTestUtilities.Repositories
{
    public class FakeMessageBus : IMessageBus
    {
        public Task PublishAsync<EventReportRequested>(EventReportRequested message, CancellationToken cancellationToken = default)
        where EventReportRequested : class
        => Task.CompletedTask;
    }
}
