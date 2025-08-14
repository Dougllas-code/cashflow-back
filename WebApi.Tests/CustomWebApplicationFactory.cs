using CashFlow.Domain.Entities;
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
        public ExpenseIdentityManager Expense { get; private set; } = default!;
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
            InsertExpenses(dbContext, user);

            dbContext.SaveChanges();
        }


        private User InsertUserTeamMember(CashFlowDbContext dbContext, IPasswordEncripter passwordEncripter, IAccessTokenGenerator tokenGenerator)
        {
             var user = UserBuilder.Build();
             var password = user.Password;

            user.Password = passwordEncripter.Encrypt(user.Password);

            dbContext.Users.Add(user);

            var token = tokenGenerator.Generate(user);

            User_Team_Member = new UserIdentityManager(user, password, token);
            return user;
        }

        private void InsertExpenses(CashFlowDbContext dbContext, User user)
        {
            var expenses = ExpenseBuilder.Collection(user); 

            dbContext.Expenses.AddRange(expenses);

            Expense = new ExpenseIdentityManager(expenses[0]);
        }
    }
}
