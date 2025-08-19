using CashFlow.Application.UseCases.User.ChangePassword;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.BaseExceptions;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities.User;
using CommonTestUtilities.InlineData;
using CommonTestUtilities.LoggerUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;

namespace UseCases.Tests.Users.ChangePassword
{
    public class ChangePasswordUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            var request = ChangePasswordRequestBuilder.Build();
            var user = UserBuilder.Build();

            var useCase = CreateUseCase(user, request.CurrentPassword);

            //Act
            async Task act() => await useCase.Execute(request);

            //Assert
            var exception = await Record.ExceptionAsync(act);
            Assert.Null(exception);
        }

        [Theory]
        [ClassData(typeof(InvalidStringInlineData))]
        public async Task Error_NewPassword_Empty(string newPassword)
        {
            //Arrange
            var request = ChangePasswordRequestBuilder.Build();
            request.NewPassword = newPassword;
            var user = UserBuilder.Build();

            var useCase = CreateUseCase(user, request.CurrentPassword);

            //Act
            var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(() => useCase.Execute(request));

            //Assert
            Assert.NotNull(exception);
            Assert.Contains(ResourceErrorMessages.PASSWORD_INVALID, exception.GetErrors());
            Assert.Single(exception.GetErrors());
        }

        [Fact]
        public async Task Error_CurrentPassword_Different()
        {
            //Arrange
            var request = ChangePasswordRequestBuilder.Build();
            var user = UserBuilder.Build();

            var useCase = CreateUseCase(user);

            //Act
            var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(() => useCase.Execute(request));

            //Assert
            Assert.NotNull(exception);
            Assert.Contains(ResourceErrorMessages.CURRENT_PASSWORD_INCORRECT, exception.GetErrors());
            Assert.Single(exception.GetErrors());
        }


        private static ChangePasswordUseCase CreateUseCase(User user, string? currentPassword = null)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var updateRepository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
            var passwordEncripter = new PasswordEncripterBuilder().Verify(currentPassword).Build();
            var unitOfWork = UnitOfWorkBuilder.Build();

            return new ChangePasswordUseCase(loggedUser, updateRepository, passwordEncripter, unitOfWork);
        }
    }
}
