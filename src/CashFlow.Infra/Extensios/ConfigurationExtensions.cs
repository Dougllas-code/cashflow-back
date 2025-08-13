using Microsoft.Extensions.Configuration;

namespace CashFlow.Infra.Extensios
{
    public static class ConfigurationExtensions
    {
        public static bool IsTestEnviroment(this IConfiguration configuration)
        {
            return configuration.GetValue<bool>("InMemoryTest");
        }
    }
}
