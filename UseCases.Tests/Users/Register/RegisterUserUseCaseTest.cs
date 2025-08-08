using CashFlow.Application.UseCases.User.Register;
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

        private static RegisterUserUseCase CreateUseCase()
        {
            var mapper = MapperBuilder.Build(); 
            var unitOfWork = UnitOfWorkBuilder.Build();
            var writeOnlyRepository = UserWriteOnlyRepositoryBuilder.Build();
            var passwordEncripter = PasswordEncripterBuilder.Build();
            var tokenGenerator = JwtTokenGeneratorBuilder.Build();
            var readOnlyRepository = new UserReadOnlyRepositoryBuilder().Build();

            return new RegisterUserUseCase(mapper, passwordEncripter, readOnlyRepository, writeOnlyRepository, unitOfWork, tokenGenerator);
        }
    }
}
