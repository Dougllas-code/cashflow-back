using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses.Expenses;
using CashFlow.Communication.Responses.User;
using CashFlow.Domain.Entities;

namespace CashFlow.Application.AutoMapper
{
    public class AutoMapping: Profile
    {
        public AutoMapping()
        {
            RequestToEntity();
            EntityToResponse();
        }

        private void RequestToEntity()
        {
            CreateMap<ExpenseRequest, Expense>();
            CreateMap<UserRequest, User>().ForMember(user => user.Password, config => config.Ignore());
        }

        private void EntityToResponse()
        {
            CreateMap<Expense, RegisterExpenseResponse>();
            CreateMap<Expense, ExpenseShortResponse>();
            CreateMap<Expense, ExpenseResponse>();
            CreateMap<User, UserProfileResponse>();
        }   
    }
}
