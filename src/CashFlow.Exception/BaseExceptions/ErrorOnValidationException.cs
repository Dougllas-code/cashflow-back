using System.Net;

namespace CashFlow.Exception.BaseExceptions
{
    public class ErrorOnValidationException: CashFlowException
    {
        private readonly List<string> _messages;
        public override int StatusCode => (int)HttpStatusCode.BadRequest;


        public ErrorOnValidationException(List<string> messages) : base(string.Empty)
        {
            _messages = messages;
        }

        public override List<string> GetErrors()
        {
            return _messages;
        }
    }
}
