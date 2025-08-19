using Bogus;
using CashFlow.Communication.Requests;

namespace CommonTestUtilities.Requests
{
    public static class ChangePasswordRequestBuilder
    {
        public static ChangePasswordRequest Build()
        {
            return new Faker<ChangePasswordRequest>()
                .RuleFor(x => x.CurrentPassword, f => f.Internet.Password(prefix: "!Aa1"))
                .RuleFor(x => x.NewPassword, f => f.Internet.Password(prefix: "!Aa1"))
                .Generate();
        } 
    }
}
