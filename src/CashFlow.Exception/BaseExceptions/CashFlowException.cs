namespace CashFlow.Exception.BaseExceptions
{
    public abstract class CashFlowException : System.Exception
    {
        public abstract int StatusCode { get; }
        public abstract List<string> GetErrors();

        protected CashFlowException(string message) : base(message) { }

    }
}
