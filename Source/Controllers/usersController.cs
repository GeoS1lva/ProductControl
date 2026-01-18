using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductControl.Application.DTOs.UsersDTOs;
using ProductControl.Application.Interfaces;
using System.Security.Claims;

namespace Source.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class usersController(IUsersAppService _usersAppService) : ControllerBase
    {
        [HttpPost]
        [Authorize(Roles = "administrator")]
        [EndpointSummary("Criar Usuário")]
        [EndpointDescription("Registra um novo usuário. Requer validação de CEP via serviço externo.")]
        public async Task<IActionResult> Create([FromBody] CreateUser newUser)
        {
            var result = await _usersAppService.CreateUserAsync(newUser);

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Value);
        }

        [HttpGet]
        [Authorize(Roles = "administrator")]
        [EndpointSummary("Listar Todos os Users")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _usersAppService.ReturnUsersAsync();

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Value);
        }

        [HttpGet("{userId}")]
        [Authorize(Roles = "administrator")]
        [EndpointSummary("Listar User Específico a partir do Id")]
        public async Task<IActionResult> GetById([FromRoute] Guid userId)
        {
            var result = await _usersAppService.ReturnUserAsync(userId);

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Value);
        }

        [HttpPatch("alterar-dados/{userId}")]
        [Authorize(Roles = "administrator")]
        [EndpointSummary("Atualizar Perfil de Usuário")]
        public async Task<IActionResult> Update([FromRoute] Guid userId, [FromBody] UpdateUser upUser)
        {
            var result = await _usersAppService.UpdateUserAsync(userId, upUser);

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return NoContent();
        }

        [HttpPatch("alterar-meus-dados")]
        [Authorize(Roles = "administrator, user")]
        [EndpointSummary("Atualizar Perfil Próprio")]
        [EndpointDescription("Permite que o usuário logado altere seus próprios dados cadastrais e endereço.")]
        public async Task<IActionResult> MyUpdate([FromBody] UpdateUser upUser)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var result = await _usersAppService.UpdateUserAsync(userId, upUser);

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return NoContent();
        }

        [HttpDelete("desativar/{userId}")]
        [Authorize(Roles = "administrator")]
        [EndpointSummary("Desativar Usuário")]
        public async Task<IActionResult> Desactive([FromRoute] Guid userId)
        {
            var result = await _usersAppService.DesactiveUser(userId);

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return NoContent();
        }

        [HttpPatch("ativar/{userId}")]
        [Authorize(Roles = "administrator")]
        [EndpointSummary("Ativar Usuário")]
        public async Task<IActionResult> Activate([FromRoute] Guid userId)
        {
            var result = await _usersAppService.ActivateUser(userId);

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return NoContent();
        }
    }
}
