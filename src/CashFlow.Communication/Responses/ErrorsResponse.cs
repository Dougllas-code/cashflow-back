namespace CashFlow.Communication.Responses
{
    public class ErrorsResponse
    {
        public List<string> Message { get; set; }

        public ErrorsResponse(string message)
        {
            Message = [message];
        }

        public ErrorsResponse(List<string> message)
        {
            Message = message;
        }
    }
}
