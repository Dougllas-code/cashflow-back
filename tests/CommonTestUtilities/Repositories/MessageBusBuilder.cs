using CashFlow.Domain.Repositories.ReportRequest;

namespace CommonTestUtilities.Repositories
{
    public static class MessageBusBuilder
    {
        public static IMessageBus Build()
        {
            var mock = new Moq.Mock<IMessageBus>();
            return mock.Object;
        }
    }
}
