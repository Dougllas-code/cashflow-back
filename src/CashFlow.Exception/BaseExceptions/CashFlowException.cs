namespace CashFlow.Exception.BaseExceptions
{
    public abstract class CashFlowException : System.Exception
    {
        protected CashFlowException(string message) : base(message)
        {

        }
    }
}
