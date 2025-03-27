using CashFlow.Communication.Responses;
using CashFlow.Exception;
using CashFlow.Exception.BaseExceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CashFlow.Api.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is CashFlowException)
            {
                HandleProjectException(context);
            }
            else
            {
                ThrowUnkowError(context);
            }
        }

        private static void HandleProjectException(ExceptionContext context)
        {
            var cashFlowException = (CashFlowException)context.Exception;
            var response = new ErrorsResponse(cashFlowException.GetErrors());

            context.HttpContext.Response.StatusCode = cashFlowException.StatusCode;
            context.Result = new ObjectResult(response);
        }

        private static void ThrowUnkowError(ExceptionContext context)
        {
            var response = new ErrorsResponse(ResourceErrorMessages.UNKNOWN_ERROR);

            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Result = new ObjectResult(response);
        }
    }
}
