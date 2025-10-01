using CashFlow.Domain.Repositories.ReportRequest;
using Moq;

namespace CommonTestUtilities.Repositories
{
    public static class ReportRequestRepositoryBuilder
    {
        public static IReportRequestRepository Build()
        {
            var mock = new Mock<IReportRequestRepository>();
            return mock.Object;
        }
    }
}
