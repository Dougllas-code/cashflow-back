using CashFlow.Domain.Entities;
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

        public UserReadOnlyRepositoryBuilder ExistActiveUserWithEmail(string email)
        {
            _mock.Setup(repo => repo.ExistActiveUserWithEmail(email)).ReturnsAsync(true);
            return this;
        }

        public UserReadOnlyRepositoryBuilder GetUserByEmail(User user)
        {
            _mock.Setup(repo => repo.GetUserByEmail(user.Email)).ReturnsAsync(user);
            return this;
        }

        public IUserReadOnlyRepository Build() => _mock.Object;
    }
}
