using CashFlow.Application.UseCases.User.GetProfile;
using CashFlow.Domain.Entities;
using CommonTestUtilities.Entities.User;
using CommonTestUtilities.LoggerUser;
using CommonTestUtilities.Mapper;

namespace UseCases.Tests.Users.GetProfile
{
    public class GetUserProfileUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            // Arrange
            var user = UserBuilder.Build();
            var useCase = CreateUseCase(user);

            // Act
            var result = await useCase.Execute();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Name, result.Name);
            Assert.Equal(user.Email, result.Email);
        }

        private static GetUserProfileUseCase CreateUseCase(User user)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var mapper = MapperBuilder.Build();

            return new GetUserProfileUseCase(loggedUser, mapper);
        }
    }
}
