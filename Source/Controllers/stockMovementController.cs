using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductControl.Application.Interfaces;

namespace Source.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class stockMovementController(IStockMovementAppService stockMovementAppService) : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles = "administrator")]
        [EndpointSummary("Histórico Global")]
        [EndpointDescription("Retorna todas as movimentações de estoque realizadas no sistema.")]
        public async Task<IActionResult> GetAll()
        {
            var result = await stockMovementAppService.GetAllStockMovementsAsync();

            if (result.Error)
                return BadRequest(result.ErrorMessage);
            
            return Ok(result.Value);
        }

        [HttpGet("product/{productId}")]
        [Authorize(Roles = "administrator")]
        [EndpointSummary("Movimentações por Produto")]
        public async Task<IActionResult> GetByProductId([FromRoute] Guid productId)
        {
            var result = await stockMovementAppService.GetByProductStockMovementsAsync(productId);

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Value);
        }

        [HttpGet("user/{userId}")]
        [Authorize(Roles = "administrator")]
        [EndpointSummary("Movimentações por User")]
        public async Task<IActionResult> GetByUserId([FromRoute] Guid userId)
        {
            var result = await stockMovementAppService.GetByUserStockMovementsAsync(userId);

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Value);
        }
    }
}
