using CashFlow.Domain.Security.Criptography;
using BC = BCrypt.Net.BCrypt;

namespace CashFlow.Infra.Security
{
    public class BCrypt : IPasswordEncripter
    {
        public string Encrypt(string password)
        {
            string passwordHash = BC.HashPassword(password);
            return passwordHash;
        }
    }
}
