using CashFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infra.DataAccess
{
    internal class CashFlowDbContext: DbContext
    {
        public DbSet<Expense> Expenses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "Server=localhost;Database=cashflowdb;Uid={yourUserName};Pwd={yourPassword};";
            var serverVersion = new MySqlServerVersion(new Version(9, 2, 0));

            optionsBuilder.UseMySql(connectionString, serverVersion);
        }
    }
}
