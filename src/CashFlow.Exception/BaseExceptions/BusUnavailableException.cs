using System.Net;

namespace CashFlow.Exception.BaseExceptions
{
    public class BusUnavailableException : CashFlowException
    {
        public override int StatusCode => (int)HttpStatusCode.ServiceUnavailable;

        private readonly string _errorMessage;

        public BusUnavailableException(string message)
            : base(message)
        {
            _errorMessage = message;
        }

        public override List<string> GetErrors()
        {
            return new List<string> { _errorMessage ?? ResourceErrorMessages.SERVICE_BUS_UNAVAILABLE };
        }
    }
}