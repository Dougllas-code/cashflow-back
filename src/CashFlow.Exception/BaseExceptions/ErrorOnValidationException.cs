namespace CashFlow.Exception.BaseExceptions
{
    public class ErrorOnValidationException: CashFlowException
    {
        public List<string> Messages { get; }

        public ErrorOnValidationException(List<string> messages) : base(string.Empty)
        {
            Messages = messages;
        }
    }
}
