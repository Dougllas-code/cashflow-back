using Bogus;
using CashFlow.Communication.Enums;
using CashFlow.Communication.Requests;

namespace CommonTestUtilities.Requests
{
    public static class RegisterExpenseRequestBuilder
    {
        public static RegisterExpenseRequest Build()
        {
            return new Faker<RegisterExpenseRequest>()
                .RuleFor(x => x.Title, f => f.Commerce.ProductName())
                .RuleFor(x => x.Description, f => f.Commerce.ProductDescription())
                .RuleFor(x => x.Date, f => f.Date.Past())
                .RuleFor(x => x.Amount, f => f.Random.Decimal(min: 1, max: 1000))
                .RuleFor(x => x.PaymentType, f => f.PickRandom<PaymentType>())
                .Generate();
        }
    }
}
