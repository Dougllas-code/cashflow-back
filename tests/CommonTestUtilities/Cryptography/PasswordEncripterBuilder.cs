using CashFlow.Domain.Security.Criptography;
using Moq;

namespace CommonTestUtilities.Cryptography
{
    public class PasswordEncripterBuilder
    {
        private readonly Mock<IPasswordEncripter> _mock;

        public PasswordEncripterBuilder()
        {
            _mock = new Mock<IPasswordEncripter>();
            _mock.Setup(config => config.Encrypt(It.IsAny<string>())).Returns("!$%#@fsdkjfb3263");
        }

        public PasswordEncripterBuilder Verify(string? password)
        {
            _mock.Setup(config => config.Verify(password ?? It.IsAny<string>(), It.IsAny<string>())).Returns(!string.IsNullOrEmpty(password));
            return this;
        }

        public IPasswordEncripter Build() => _mock.Object;
    }
}
