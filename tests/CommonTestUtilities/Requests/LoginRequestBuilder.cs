using Bogus;
using CashFlow.Communication.Requests;

namespace CommonTestUtilities.Requests
{
    public static class LoginRequestBuilder
    {
        public static LoginRequest Build(string? email = null)
        {
            return new Faker<LoginRequest>()
                .RuleFor(x => x.Email, f => email ?? f.Internet.Email())
                .RuleFor(x => x.Password, f => f.Internet.Password(prefix: "!Aa1"))
                .Generate();
        }
    }
}
