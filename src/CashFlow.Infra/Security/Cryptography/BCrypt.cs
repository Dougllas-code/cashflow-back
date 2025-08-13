using CashFlow.Domain.Security.Criptography;
using BC = BCrypt.Net.BCrypt;

namespace CashFlow.Infra.Security.Cryptography
{
    public class BCrypt : IPasswordEncripter
    {
        public string Encrypt(string password)
        {
            string passwordHash = BC.HashPassword(password);
            return passwordHash;
        }

        public bool Verify(string password, string encryptedPassword) => BC.Verify(password, encryptedPassword);
    }
}
