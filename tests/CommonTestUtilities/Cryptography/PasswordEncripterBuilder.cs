using CashFlow.Domain.Security.Criptography;
using Moq;

namespace CommonTestUtilities.Cryptography
{
    public static class PasswordEncripterBuilder
    {
        public static IPasswordEncripter Build()
        {
            var mock = new Mock<IPasswordEncripter>();
            mock.Setup(config => config.Encrypt(It.IsAny<string>())).Returns("!$%#@fsdkjfb3263");

            return mock.Object;
        }
    }
}
