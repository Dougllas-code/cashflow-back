using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Services.LoggedUser;

namespace CashFlow.Application.UseCases.User.Delete
{
    public class DeleteUserUseCase : IDeleteUserUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserWriteOnlyRepository _userRepository;

        public DeleteUserUseCase(
            ILoggedUser loggedUser, 
            IUnitOfWork unitOfWork,
            IUserWriteOnlyRepository userRepository)
        {
            _loggedUser = loggedUser;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        public async Task Execute()
        {
            var user = await _loggedUser.Get();

            await _userRepository.Delete(user);

            await _unitOfWork.Commit();
        }
    }
}
