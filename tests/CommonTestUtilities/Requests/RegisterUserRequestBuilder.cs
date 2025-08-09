using Bogus;
using CashFlow.Communication.Requests;

namespace CommonTestUtilities.Requests
{
    public static class RegisterUserRequestBuilder
    {
        public static UserRequest Build()
        {
            return new Faker<UserRequest>()
                .RuleFor(x => x.Name, f => f.Person.FirstName)
                .RuleFor(x => x.Email, (f, user) => f.Internet.Email(user.Name))
                .RuleFor(x => x.Password, f => f.Internet.Password(prefix: "!Aa1"))
                .Generate();
        }
    }
}
