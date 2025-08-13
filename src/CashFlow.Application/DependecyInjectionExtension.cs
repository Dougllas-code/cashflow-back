using CashFlow.Application.AutoMapper;
using CashFlow.Application.UseCases.Expenses.Delete;
using CashFlow.Application.UseCases.Expenses.GetAll;
using CashFlow.Application.UseCases.Expenses.GetById;
using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Application.UseCases.Expenses.Report.Excel;
using CashFlow.Application.UseCases.Expenses.Report.PDF;
using CashFlow.Application.UseCases.Expenses.Update;
using CashFlow.Application.UseCases.Login.DoLogin;
using CashFlow.Application.UseCases.User.Register;
using CashFlow.Domain.Services.LoggedUser;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.Application
{
    public static class DependecyInjectionExtension
    {
        public static void AddApplication(this IServiceCollection services)
        {
            AddAutoMaper(services);
            AddUseCases(services);
        }

        private static void AddAutoMaper(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapping));
        }

        private static void AddUseCases(IServiceCollection services)
        {
            #region Expenses
            services.AddScoped<IRegisterExpenseUseCase, RegisterExpenseUseCase>();
            services.AddScoped<IGetAllExpensesUseCase, GetAllExpensesUseCase>();
            services.AddScoped<IGetByIdUseCase, GetByIdUseCase>();
            services.AddScoped<IDeleteExpenseUseCase, DeleteExpenseUseCase>();
            services.AddScoped<IUpdateExpenseUseCase, UpdateExpenseUseCase>();
            #endregion

            #region Reports
            services.AddScoped<IGenerateExpensesReportExcelUseCase, GenerateExpensesReportExcelUseCase>();
            services.AddScoped<IGenerateExpensesReportPdfUseCase, GenerateExpensesReportPdfUseCase>();
            #endregion

            #region User
            services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
            #endregion

            #region Login
            services.AddScoped<ILoginUseCase, LoginUseCase>();
            #endregion
        }
    }
}
