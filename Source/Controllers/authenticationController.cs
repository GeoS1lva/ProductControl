using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductControl.Application.DTOs.UsersDTOs;
using ProductControl.Application.Interfaces;
using ProductControl.Domain.Entities;
using ProductControl.Domain.Interfaces.Repositories;
using System.IdentityModel.Tokens.Jwt;

namespace Source.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class authenticationController(IUsersAppService usersAppService, IBlackListRepository blackListRepository) : ControllerBase
    {
        [HttpPost("login")]
        [AllowAnonymous]
        [EndpointSummary("Autenticar Usuário")]
        [EndpointDescription("Valida as credenciais (username/password) e retorna um Token JWT para acesso aos recursos protegidos.")]
        public async Task<IActionResult> Login([FromBody] RequestLogin request)
        {
            var result = await usersAppService.LoginAsync(request);

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Value);
        }

        [HttpPost("logout")]
        [Authorize]
        [EndpointSummary("Encerrar Sessão")]
        [EndpointDescription("Invalida o token atual adicionando o JTI à Blacklist no banco de dados.")]
        public async Task<IActionResult> Logout()
        {
            var jti = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

            var expClaim = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;
            var expiry = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expClaim!)).UtcDateTime;

            if (!string.IsNullOrEmpty(jti))
            {
                await blackListRepository.RevokeTokenAsync(new RevokedToken(jti, expiry));
            }

            return Ok(new { message = "Logout realizado com sucesso." });
        }
    }
}
