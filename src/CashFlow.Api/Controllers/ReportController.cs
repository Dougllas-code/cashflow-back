using CashFlow.Application.UseCases.Expenses.Report;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses.Expenses;
using CashFlow.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.ADMIN)]
    public class ReportController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(RegisterExpenseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetExcel(
            [FromServices] IGenerateExpensesReportUseCase useCase,
            [FromBody] ReportRequest request) 
        {
            var response = await useCase.Execute(request);

            if (response is not null)
            {
                return Ok(response);
            }

            return NoContent();
        }
    }
}
