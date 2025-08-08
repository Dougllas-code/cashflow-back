using CashFlow.Application.UseCases.User.Register;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;

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

        private RegisterUserUseCase CreateUseCase()
        {
            var mapper = MapperBuilder.Build(); 
            var unitOfWork = UnitOfWorkBuilder.Build();
            var writeOnlyRepository = UserWriteOnlyRepositoryBuilder.Build();

            return new RegisterUserUseCase(mapper, null, null, writeOnlyRepository, unitOfWork, null);
        }
    }
}
