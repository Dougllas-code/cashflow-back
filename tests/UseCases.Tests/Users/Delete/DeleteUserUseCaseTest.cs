using CashFlow.Application.UseCases.User.Delete;
using CashFlow.Domain.Entities;
using CommonTestUtilities.Entities.User;
using CommonTestUtilities.LoggerUser;
using CommonTestUtilities.Repositories;

namespace UseCases.Tests.Users.Delete
{
    public class DeleteUserUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            // Arrange
            var user = UserBuilder.Build();
            var useCase = CreateUseCase(user);

            // Act
            async Task act() => await useCase.Execute();

            //Assert
            var exception = await Record.ExceptionAsync(act);
            Assert.Null(exception);
        }

        private static DeleteUserUseCase CreateUseCase(User user)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var unitOfWork = UnitOfWorkBuilder.Build();
            var userRepository = UserWriteOnlyRepositoryBuilder.Build();

            return new DeleteUserUseCase(loggedUser, unitOfWork, userRepository);
        }
    }
}
