using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.User;
using Moq;

namespace CommonTestUtilities.Repositories
{
    public class UserUpdateOnlyRepositoryBuilder
    {
        private readonly Mock<IUserUpdateOnlyRepository> _repositoryMock;

        public UserUpdateOnlyRepositoryBuilder()
        {
            _repositoryMock = new Mock<IUserUpdateOnlyRepository>();
        }

        public UserUpdateOnlyRepositoryBuilder GetById(User? user)
        {
            if (user is not null)
                _repositoryMock.Setup(repo => repo.GetById(user.Id)).ReturnsAsync(user);

            return this;
        }

        public IUserUpdateOnlyRepository Build() => _repositoryMock.Object;
    }
}
