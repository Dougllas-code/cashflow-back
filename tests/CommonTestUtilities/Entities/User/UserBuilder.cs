using Bogus;
using CashFlow.Domain.Enums;
using CommonTestUtilities.Cryptography;

namespace CommonTestUtilities.Entities.User
{
    public class UserBuilder
    {
        public static CashFlow.Domain.Entities.User Build(string role = Roles.TEAM_MEMBER)
        {
            var passwordEncripter = new PasswordEncripterBuilder().Build();

            return new Faker<CashFlow.Domain.Entities.User>()
                .RuleFor(x => x.Id, _ => 1)
                .RuleFor(x => x.Name, f => f.Person.FirstName)
                .RuleFor(x => x.Email, (f, user) => f.Internet.Email(user.Name))
                .RuleFor(x => x.Password, (_, user) => passwordEncripter.Encrypt(user.Password))
                .RuleFor(x => x.UserIdentifier, _ => Guid.NewGuid())
                .RuleFor(x => x.Role, _ => role)
                .Generate();
        }
    }
}
