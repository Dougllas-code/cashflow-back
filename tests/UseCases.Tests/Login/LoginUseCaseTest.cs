using CashFlow.Application.UseCases.Login.DoLogin;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.BaseExceptions;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities.User;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Token;

namespace UseCases.Tests.Login
{
    public class LoginUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            var user = UserBuilder.Build();

            var request = LoginRequestBuilder.Build(user.Email);
            var useCase = CreateLoginUseCase(user, request.Password);

            //Act
            var result = await useCase.Execute(request);

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Token);
            Assert.Equal(user.Name, result.Name);
            Assert.NotEmpty(result.Token);
        }

        [Fact]
        public async Task Error_User_Not_Found()
        {
            //Arrange
            var user = UserBuilder.Build();

            var request = LoginRequestBuilder.Build();
            var useCase = CreateLoginUseCase(user, request.Password);

            //Act
            var exception = await Assert.ThrowsAsync<InvalidLoginException>(() => useCase.Execute(request));

            //Assert
            Assert.NotNull(exception);
            Assert.Contains(ResourceErrorMessages.EMAIL_OR_PASSWORD_INVALID, exception.GetErrors());
            Assert.Single(exception.GetErrors());
        }

        [Fact]
        public async Task Error_Invalid_Password()
        {
            //Arrange
            var user = UserBuilder.Build();

            var request = LoginRequestBuilder.Build(user.Email);
            var useCase = CreateLoginUseCase(user);

            //Act
            var exception = await Assert.ThrowsAsync<InvalidLoginException>(() => useCase.Execute(request));

            //Assert
            Assert.NotNull(exception);
            Assert.Contains(ResourceErrorMessages.EMAIL_OR_PASSWORD_INVALID, exception.GetErrors());
            Assert.Single(exception.GetErrors());
        }

        private static LoginUseCase CreateLoginUseCase(User user, string? requestPassword = null)
        {
            var userReadOnlyRepository = new UserReadOnlyRepositoryBuilder().GetUserByEmail(user).Build();
            var passwordEncripter = new PasswordEncripterBuilder().Verify(requestPassword).Build();
            var tokenGenerator = JwtTokenGeneratorBuilder.Build();

            return new LoginUseCase(userReadOnlyRepository, passwordEncripter, tokenGenerator);
        }
    }
}
