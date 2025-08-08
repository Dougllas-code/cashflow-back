using CashFlow.Domain.Repositories.User;
using Moq;

namespace CommonTestUtilities.Repositories
{
    public class UserReadOnlyRepositoryBuilder
    {
        private readonly Mock<IUserReadOnlyRepository> _mock;

        public UserReadOnlyRepositoryBuilder()
        {
            _mock = new Mock<IUserReadOnlyRepository>();
        }

        public IUserReadOnlyRepository Build() => _mock.Object;
    }
}
