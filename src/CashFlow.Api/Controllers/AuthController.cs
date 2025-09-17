using CashFlow.Application.UseCases.CheckToken;
using CashFlow.Communication.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        [HttpGet("validate-token")]
        [ProducesResponseType(typeof(CheckTokenResponse), StatusCodes.Status200OK)]
        public IActionResult ValidateToken(
            [FromServices] ICheckTokenUseCase useCase)
        {
            var result = useCase.Execute();
            if (result.IsValid)
                return Ok(result);

            return Unauthorized(result);
        }
    }
}
