namespace CashFlow.Communication.Responses
{
    public class RegisterExpenseResponse
    {
        public string Title { get; set; }

        public RegisterExpenseResponse(string title)
        {
            Title = title;
        }
    }
}
