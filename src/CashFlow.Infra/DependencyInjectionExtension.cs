using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Repositories.ReportRequest;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Security.Criptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Infra.DataAccess;
using CashFlow.Infra.DataAccess.Repositories;
using CashFlow.Infra.Extensios;
using CashFlow.Infra.Security.Tokens;
using CashFlow.Infra.Services.LoggedUser;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.Infra
{
    public static class DependencyInjectionExtension
    {
        public static void AddInfraStructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPasswordEncripter, Security.Cryptography.BCrypt>();
            services.AddScoped<ILoggedUser, LoggedUser>();

            AddToken(services, configuration);
            AddRepositories(services);
            AddMassTransit(services, configuration);

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
            services.AddScoped<IUserUpdateOnlyRepository, UserRepository>();
            #endregion

            #region Report Request
            services.AddScoped<IReportRequestRepository, ReportRequestRepository>();
            services.AddScoped<IMessageBus, ReportRequestRepository>();
            #endregion
        }

        private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Connection");
            var serverVersion = ServerVersion.AutoDetect(connectionString);

            services.AddDbContext<CashFlowDbContext>(config => config.UseMySql(connectionString, serverVersion));
        }

        private static void AddMassTransit(IServiceCollection services, IConfiguration configuration)
        {
            var connection = configuration.GetSection("Settings:RabbitMq:Host").Value!;
            var username = configuration.GetSection("Settings:RabbitMq:Username").Value!;
            var password = configuration.GetSection("Settings:RabbitMq:Password").Value!;

            services.AddMassTransit(busConfigurator =>
            {
                busConfigurator.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(connection, host =>
                    {
                        host.Username(username);
                        host.Password(password);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

        }
    }
}
