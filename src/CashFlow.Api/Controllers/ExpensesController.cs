using CashFlow.Application.UseCases.Expenses.Delete;
using CashFlow.Application.UseCases.Expenses.GetAll;
using CashFlow.Application.UseCases.Expenses.GetById;
using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Application.UseCases.Expenses.Update;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Communication.Responses.Expenses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExpensesController : ControllerBase
    {

        [HttpPost]
        [ProducesResponseType(typeof(RegisterExpenseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorsResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorsResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register(
            [FromServices] IRegisterExpenseUseCase useCase,
            [FromBody] ExpenseRequest request
        )
        {
            var response = await useCase.Execute(request);
            return Created(string.Empty, response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ExpensesResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAll([FromServices] IGetAllExpensesUseCase usercase)
        {
            var response = await usercase.Execute();

            if (response.Expenses.Count == 0)
                return NoContent();

            return Ok(response);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ExpenseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorsResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(
            [FromServices] IGetByIdUseCase useCase,
            [FromRoute] long id
        )
        {
            var response = await useCase.Execute(id);
            if (response == null)
                return NotFound();

            return Ok(response);
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorsResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            [FromServices] IDeleteExpenseUseCase useCase,
            [FromRoute] long id
        )
        {
            await useCase.Execute(id);

            return NoContent();
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorsResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorsResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(
            [FromServices] IUpdateExpenseUseCase useCase,
            [FromRoute] long id,
            [FromBody] ExpenseRequest request
        )
        {
            await useCase.Execute(id, request);
          
            return NoContent();
        }
    }
}
