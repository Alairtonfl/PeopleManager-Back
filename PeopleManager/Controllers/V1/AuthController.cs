using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeopleManager.API.Models;
using PeopleManager.Application.DTOs.Responses;
using PeopleManager.Application.Interfaces.Services;
using System.Security.Claims;

namespace PeopleManager.API.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        
        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] AuthRequestJson request, CancellationToken cancellationToken)
        {
            PeopleResponseDto peopleResponseDto = await _authService.AuthenticateAsync(request.Cpf, request.Password, cancellationToken);
            string token = _authService.GenerateJwtToken(peopleResponseDto);

            CookieOptions cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.UtcNow.AddDays(1)
            };

            Response.Cookies.Append("jwt_token", token, cookieOptions);

            return Ok(new ApiResponse<PeopleResponseDto>
            {
                Data = peopleResponseDto,
                Success = true
            });
        }

        [HttpGet("me")]
        public IActionResult GetCurrentUser(CancellationToken cancellationToken)
        {
            string? userId = User.FindFirst("UserId")?.Value;
            string? name = User.FindFirst(ClaimTypes.Name)?.Value;

            if (userId == null || name == null)
                return Unauthorized();

            PeopleResponseDto peopleResponseDto = new PeopleResponseDto
            {
                Id = Convert.ToInt64(userId),
                Name = name
            };

            return Ok(new ApiResponse<PeopleResponseDto>
            {
                Data = peopleResponseDto,
                Success = true
            });
        }
    }
}
