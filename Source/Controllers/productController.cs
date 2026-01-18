using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductControl.Application.DTOs.ProductDTOs;
using ProductControl.Application.DTOs.UsersDTOs;
using ProductControl.Application.Interfaces;
using ProductControl.Application.Services;
using ProductControl.Domain.Entities;
using System.Security.Claims;

namespace Source.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class productController(IProductAppService productAppService) : ControllerBase
    {
        [HttpPost]
        [Authorize(Roles = "administrator, user")]
        [EndpointSummary("Cadastrar Produto")]
        [EndpointDescription("Cria um novo produto e o ativa automaticamente no sistema.")]
        public async Task<IActionResult> Create([FromBody] CreateProduct newProduct)
        {
            var result = await productAppService.CreateProductAsync(newProduct);

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Value);
        }

        [HttpGet]
        [Authorize(Roles = "administrator, user")]
        [EndpointSummary("Listar Todos os Produtos")]
        public async Task<IActionResult> GetAll()
        {
            var result = await productAppService.ReturnProductsAsync();

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Value);
        }

        [HttpGet("{productId}")]
        [Authorize(Roles = "administrator, user")]
        [EndpointSummary("Listar Produto Específico a partir do Id")]
        public async Task<IActionResult> GetById([FromRoute] Guid productId)
        {
            var result = await productAppService.ReturnProductAsync(productId);

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Value);
        }

        [HttpPatch("alterar-dados/{productId}")]
        [Authorize(Roles = "administrator, user")]
        [EndpointSummary("Atualizar dados de Produto")]
        [EndpointDescription("Atualiza dados de Produto a partir do Id")]
        public async Task<IActionResult> Update([FromRoute] Guid productId, [FromBody] UpdateProduct upProduct)
        {
            var result = await productAppService.UpdateProductAsync(productId, upProduct);

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return NoContent();
        }

        [HttpDelete("desativar/{productId}")]
        [Authorize(Roles = "administrator")]
        [EndpointSummary("Desativar Produto")]
        [EndpointDescription("Desativa Produto Ativo. Valida antes se o produto está ativo")]
        public async Task<IActionResult> Desactive([FromRoute] Guid productId)
        {
            var result = await productAppService.DesactiveProduct(productId);

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return NoContent();
        }

        [HttpPatch("ativar/{productId}")]
        [Authorize(Roles = "administrator")]
        [EndpointSummary("Ativar Produto")]
        [EndpointDescription("Ativa Produto Inativado. Valida antes se o produto está inativo")]
        public async Task<IActionResult> Activate([FromRoute] Guid productId)
        {
            var result = await productAppService.ActivateProduct(productId);

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return NoContent();
        }

        [HttpPost("adicionar-estoque/{productId}")]
        [Authorize(Roles = "administrator, user")]
        [EndpointSummary("Entrada de Estoque")]
        [EndpointDescription("Aumenta a quantidade em estoque de um produto e registra a movimentação.")]
        public async Task<IActionResult> AddStock([FromRoute] Guid productId, [FromBody] int amount)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var result = await productAppService.AddProductAsync(productId, userId, amount);

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return NoContent();
        }

        [HttpPost("remover-estoque/{productId}")]
        [Authorize(Roles = "administrator, user")]
        [EndpointSummary("Saída de Estoque")]
        [EndpointDescription("Diminui a quantidade em estoque de um produto. Valida se há saldo suficiente.")]
        public async Task<IActionResult> RemoveStock([FromRoute] Guid productId, [FromBody] int amount)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var result = await productAppService.RemoveProductAsync(productId, userId, amount);

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return NoContent();
        }
    }
}
