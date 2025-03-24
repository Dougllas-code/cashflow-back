using System.Net;

namespace CashFlow.Exception.BaseExceptions
{
    public class NotFoundException : CashFlowException
    {
        public override int StatusCode => (int)HttpStatusCode.NotFound;
        public NotFoundException(string message) : base(message) { }

        public override List<string> GetErrors()
        {
            return [Message];
        }
    }
}
