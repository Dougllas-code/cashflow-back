using Bogus;
using CashFlow.Communication.Requests;

namespace CommonTestUtilities.Requests
{
    public static class UpdateUserRequestBuilder
    {
        public static UpdateUserRequest Build()
        {
            return new Faker<UpdateUserRequest>()
                .RuleFor(x => x.Name, f => f.Person.FirstName)
                .RuleFor(x => x.Email, (f, user) => f.Internet.Email(user.Name))
                .Generate();
        }
    }
}
