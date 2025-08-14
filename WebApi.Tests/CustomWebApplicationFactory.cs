using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.Criptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Infra.DataAccess;
using CommonTestUtilities.Entities.User;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private Expense _expense;
        private User _user;
        private string _password;
        private string _token;

        public string GetEmail() => _user.Email;
        public string GetName() => _user.Name;
        public string GetPassword() => _password;
        public string GetToken() => _token;
        public long GetExpenseId() => _expense.Id;

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

                    StartDatabase(dbContext, passwordEncripter);

                    _token = tokenGenerator.Generate(_user);
                });
        }

        private void StartDatabase(CashFlowDbContext dbContext, IPasswordEncripter passwordEncripter)
        {
            InsertUser(dbContext, passwordEncripter);
            InsertExpenses(dbContext);

            dbContext.SaveChanges();
        }


        private void InsertUser(CashFlowDbContext dbContext, IPasswordEncripter passwordEncripter)
        {
            _user = UserBuilder.Build();
            _password = _user.Password;

            _user.Password = passwordEncripter.Encrypt(_user.Password);

            dbContext.Users.Add(_user);
            
        }

        private void InsertExpenses(CashFlowDbContext dbContext)
        {
            var expenses = ExpenseBuilder.Collection(_user);
            _expense = expenses.FirstOrDefault()!;

            dbContext.Expenses.AddRange(expenses);
        }
    }
}
