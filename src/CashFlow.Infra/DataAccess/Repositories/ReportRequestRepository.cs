using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.ReportRequest;
using CashFlow.Exception.BaseExceptions;
using MassTransit;
using RabbitMQ.Client.Exceptions;

namespace CashFlow.Infra.DataAccess.Repositories
{
    internal class ReportRequestRepository : IReportRequestRepository, IMessageBus
    {
        private readonly CashFlowDbContext _dbContext;
        private readonly IBus _bus;

        public ReportRequestRepository(
            CashFlowDbContext dbContext,
            IBus bus)
        {
            _dbContext = dbContext;
            _bus = bus;
        }
        public async Task Create(ReportRequests reportRequest)
        {
            await _dbContext.ReportRequests.AddAsync(reportRequest);
        }

        public async Task PublishAsync<EventReportRequested>(EventReportRequested message, CancellationToken cancellationToken = default) where EventReportRequested : class
        {
            try
            {
                await _bus.Publish(message!, cancellationToken);
            }
            catch (System.Exception ex)
            {
                throw new BusUnavailableException(ex.Message);
            }
        }
    }
}
