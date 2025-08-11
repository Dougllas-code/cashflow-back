using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Security.Criptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Infra.DataAccess;
using CashFlow.Infra.DataAccess.Repositories;
using CashFlow.Infra.Extensios;
using CashFlow.Infra.Security.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.Infra
{
    public static class DependencyInjectionExtension
    {
        public static void AddInfraStructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddToken(services, configuration);
            AddRepositories(services);

            services.AddScoped<IPasswordEncripter, Security.Cryptography.BCrypt>();

            if (!configuration.IsTestEnviroment())
            {
                AddDbContext(services, configuration);
            }
        }

        private static void AddToken(IServiceCollection services, IConfiguration configuration)
        {
            var expirationInMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpiresMinutes");
            var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");

            services.AddScoped<IAccessTokenGenerator>(config => new JwtTokenGenerator(expirationInMinutes, signingKey!));
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            #region Expenses
            services.AddScoped<IExpensesWriteOnlyRepository, ExpenseRepository>();
            services.AddScoped<IExpensesReadOnlyRepository, ExpenseRepository>();
            services.AddScoped<IExpensesUpdateOnlyRepository, ExpenseRepository>();
            #endregion

            #region User
            services.AddScoped<IUserReadOnlyRepository, UserRepository>();
            services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
            #endregion
        }

        private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Connection");
            var serverVersion = ServerVersion.AutoDetect(connectionString);

            services.AddDbContext<CashFlowDbContext>(config => config.UseMySql(connectionString, serverVersion));
        }
    }
}
