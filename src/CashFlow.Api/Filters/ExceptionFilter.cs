using CashFlow.Communication.Responses;
using CashFlow.Exception;
using CashFlow.Exception.BaseExceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace CashFlow.Api.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

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

        private void HandleProjectException(ExceptionContext context)
        {
            var cashFlowException = (CashFlowException)context.Exception;
            var response = new ErrorsResponse(cashFlowException.GetErrors());

            _logger.LogWarning("CashFlow exception occurred. Type: {ExceptionType}, StatusCode: {StatusCode}, Errors: {Errors}",
                cashFlowException.GetType().Name,
                cashFlowException.StatusCode,
                string.Join(", ", cashFlowException.GetErrors()));

            context.HttpContext.Response.StatusCode = cashFlowException.StatusCode;
            context.Result = new ObjectResult(response);
        }

        private void ThrowUnkowError(ExceptionContext context)
        {
            var response = new ErrorsResponse(ResourceErrorMessages.UNKNOWN_ERROR);

            _logger.LogError(context.Exception, "Unknown error occurred. ExceptionType: {ExceptionType}, Message: {Message}",
                context.Exception.GetType().Name,
                context.Exception.Message);

            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Result = new ObjectResult(response);
        }
    }
}
