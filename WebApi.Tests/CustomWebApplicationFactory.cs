using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;
using CashFlow.Domain.Security.Criptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Infra.DataAccess;
using CommonTestUtilities.Entities.User;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Tests.Resources;

namespace WebApi.Tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        public ExpenseIdentityManager Expense_Team_Member { get; private set; } = default!;
        public ExpenseIdentityManager Expense_Admin { get; private set; } = default!;
        public UserIdentityManager User_Team_Member { get; private set; } = default!;
        public UserIdentityManager User_Admin { get; private set; } = default!;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing")
                .ConfigureServices(services =>
                {
                    var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                    services.AddDbContext<CashFlowDbContext>(config =>
                    {
                        config.UseInMemoryDatabase("InMemoryDbForTesting");
                        config.UseInternalServiceProvider(provider);
                    });

                    var scope = services.BuildServiceProvider().CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<CashFlowDbContext>();
                    var passwordEncripter = scope.ServiceProvider.GetRequiredService<IPasswordEncripter>();
                    var tokenGenerator = scope.ServiceProvider.GetRequiredService<IAccessTokenGenerator>();

                    StartDatabase(dbContext, passwordEncripter, tokenGenerator);
                });
        }

        private void StartDatabase(CashFlowDbContext dbContext, IPasswordEncripter passwordEncripter, IAccessTokenGenerator tokenGenerator)
        {
            var user = InsertUserTeamMember(dbContext, passwordEncripter, tokenGenerator);
            var expenseTeamMember = InsertExpenses(dbContext, user, expenseId: 1);

            Expense_Team_Member = new ExpenseIdentityManager(expenseTeamMember);

            var userAdmin = InsertUserAdmin(dbContext, passwordEncripter, tokenGenerator);
            var expenseAdmin = InsertExpenses(dbContext, userAdmin, expenseId: 2);

            Expense_Admin = new ExpenseIdentityManager(expenseAdmin);

            dbContext.SaveChanges();
        }


        private User InsertUserTeamMember(CashFlowDbContext dbContext, IPasswordEncripter passwordEncripter, IAccessTokenGenerator tokenGenerator)
        {
            var user = UserBuilder.Build();
            user.Id = 1;

            var password = user.Password;

            user.Password = passwordEncripter.Encrypt(user.Password);

            dbContext.Users.Add(user);

            var token = tokenGenerator.Generate(user);

            User_Team_Member = new UserIdentityManager(user, password, token);
            return user;
        }

        private User InsertUserAdmin(CashFlowDbContext dbContext, IPasswordEncripter passwordEncripter, IAccessTokenGenerator tokenGenerator)
        {
            var user = UserBuilder.Build(Roles.ADMIN);
            user.Id = 2;

            var password = user.Password;

            user.Password = passwordEncripter.Encrypt(user.Password);

            dbContext.Users.Add(user);

            var token = tokenGenerator.Generate(user);

            User_Team_Member = new UserIdentityManager(user, password, token);
            return user;
        }

        private Expense InsertExpenses(CashFlowDbContext dbContext, User user, long expenseId)
        {
            var expense = ExpenseBuilder.Build(user);
            expense.Id = expenseId;

            dbContext.Expenses.AddRange(expense);

            return expense;
        }
    }
}
