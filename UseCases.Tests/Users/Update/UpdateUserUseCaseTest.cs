using CashFlow.Application.UseCases.User.Update;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.BaseExceptions;
using CommonTestUtilities.Entities.User;
using CommonTestUtilities.InlineData;
using CommonTestUtilities.LoggerUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;

namespace UseCases.Tests.Users.Update
{
    public class UpdateUserUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            var user = UserBuilder.Build();
            var request = UpdateUserRequestBuilder.Build();

            var useCase = CreateUseCase(user);

            //Act
            await useCase.Execute(request);

            //Assert
            Assert.Equal(user.Name, request.Name);
            Assert.Equal(user.Email, request.Email);
        }

        [Theory]
        [ClassData(typeof(InvalidStringInlineData))]
        public async Task Error_Name_empty(string name)
        {
            //Arrange
            var user = UserBuilder.Build();
            var request = UpdateUserRequestBuilder.Build();
            request.Name = name;

            var useCase = CreateUseCase(user);

            //Act
            var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(() => useCase.Execute(request));

            //Assert
            Assert.NotNull(exception);
            Assert.Contains(ResourceErrorMessages.NAME_EMPTY, exception.GetErrors());
            Assert.Single(exception.GetErrors());
        }

        [Fact]
        public async Task Error_Email_Already_Exist()
        {
            //Arrange
            var user = UserBuilder.Build();
            var request = UpdateUserRequestBuilder.Build();

            var useCase = CreateUseCase(user, request.Email);

            //Act
            var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(() => useCase.Execute(request));

            //Assert
            Assert.NotNull(exception);
            Assert.Contains(ResourceErrorMessages.EMAIL_ALREADY_REGISTERED, exception.GetErrors());
            Assert.Single(exception.GetErrors());
        }

        public static UpdateUserUseCase CreateUseCase(User user, string? email = null)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var readOnlyRepository = new UserReadOnlyRepositoryBuilder();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var updateOnlyRepository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();

            if (email is not null)
            {
                readOnlyRepository.ExistActiveUserWithEmail(email);
            }

            return new UpdateUserUseCase(loggedUser, readOnlyRepository.Build(), unitOfWork, updateOnlyRepository);
        }
    }
}
