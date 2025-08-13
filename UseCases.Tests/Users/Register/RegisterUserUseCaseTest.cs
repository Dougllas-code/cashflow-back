using CashFlow.Application.UseCases.User.Register;
using CashFlow.Exception;
using CashFlow.Exception.BaseExceptions;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Token;

namespace UseCases.Tests.Users.Register
{
    public class RegisterUserUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            var request = RegisterUserRequestBuilder.Build();
            var useCase = CreateUseCase();

            //Act
            var result = await useCase.Execute(request);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(result.Name, request.Name);
            Assert.NotNull(result.Token);
            Assert.NotEmpty(result.Token);
        }

        [Fact]
        public async Task Error_Name_Empty()
        {
            //Arrange
            var request = RegisterUserRequestBuilder.Build();
            request.Name = string.Empty;

            var useCase = CreateUseCase();

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
            var request = RegisterUserRequestBuilder.Build();

            var useCase = CreateUseCase(request.Email);

            //Act
            var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(() => useCase.Execute(request));

            //Assert
            Assert.NotNull(exception);
            Assert.Contains(ResourceErrorMessages.EMAIL_ALREADY_REGISTERED, exception.GetErrors());
            Assert.Single(exception.GetErrors());
        }

        private static RegisterUserUseCase CreateUseCase(string? email = null)
        {
            var mapper = MapperBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var writeOnlyRepository = UserWriteOnlyRepositoryBuilder.Build();
            var passwordEncripter = new PasswordEncripterBuilder();
            var tokenGenerator = JwtTokenGeneratorBuilder.Build();
            var readOnlyRepository = new UserReadOnlyRepositoryBuilder();

            if (!string.IsNullOrWhiteSpace(email))
            {
                readOnlyRepository.ExistActiveUserWithEmail(email);
            }

            return new RegisterUserUseCase(mapper, passwordEncripter.Build(), readOnlyRepository.Build(), writeOnlyRepository, unitOfWork, tokenGenerator);
        }
    }
}
