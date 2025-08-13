using Bogus;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;

namespace CommonTestUtilities.Entities.User
{
    public class ExpenseBuilder
    {
        public static List<Expense> Collection(CashFlow.Domain.Entities.User user, uint count = 2)
        {
            var list = new List<Expense>();
            if (count == 0)
                count = 1;

            var expenseId = 1;

            for (var i = 0; i < count; i++)
            {
                var expense = Build(user);
                expense.Id = expenseId++;

                list.Add(expense);
            }

            return list;
        }

        public static Expense Build(CashFlow.Domain.Entities.User user)
        {
            return new Faker<Expense>()
                .RuleFor(x => x.Id, _ => 1)
                .RuleFor(x => x.Title, f => f.Commerce.ProductName())
                .RuleFor(x => x.Description, f => f.Commerce.ProductDescription())
                .RuleFor(x => x.Amount, f => f.Random.Decimal(min: 1, max: 1000))
                .RuleFor(x => x.Date, f => f.Date.Past())
                .RuleFor(x => x.PaymentType, f => f.PickRandom<PaymentType>())
                .RuleFor(x => x.UserId, _ => user.Id)
                .Generate();
        }
    }
}
