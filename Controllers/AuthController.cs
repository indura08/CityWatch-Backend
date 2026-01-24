using cityWatch_Project.DTOs.Auth;
using cityWatch_Project.DTOs.Users;
using cityWatch_Project.Enums;
using cityWatch_Project.Models;
using cityWatch_Project.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cityWatch_Project.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        //this is to test the api is working succsfully
        [HttpGet("help/hi")]
        [Authorize(Roles="Admin")]
        public IActionResult SayHi()
        {
            return Ok("Hellow from citywatch API");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("register/admin")]
        public async Task<ActionResult<LoginResponseDTO>> RegisterAdmin(RegisterDTO registerDto)
        {
            var result = await _authService.RegisterAsync(registerDto, Enums.Role.Admin);
            if (result.Error) 
            {
                var response = new LoginResponseDTO
                {
                    Error = true,
                    ErrorMessage = result.ErrorMessage,
                    Token = "",
                    RefreshToken = result.RefreshToken
                };
                return StatusCode(500, response);
            }

            return Ok(new LoginResponseDTO
            {
                Error = false,
                Token = result.Token!,
                ErrorMessage = "",
                RefreshToken = result.RefreshToken
            });
        }

        [HttpPost("register/citizen")]
        public async Task<ActionResult<LoginResponseDTO>> RegisterCitizen(RegisterDTO registerDto)
        {
            var result = await _authService.RegisterAsync(registerDto, Enums.Role.Citizen);
            if (result.Error)
            {
                var response = new LoginResponseDTO
                {
                    Error = true,
                    ErrorMessage = result.ErrorMessage,
                    Token = ""
                };
                return StatusCode(500, response);
            }

            return Ok(new LoginResponseDTO
            {
                Error = false,
                Token = result.Token!,
                ErrorMessage = "no error",
                RefreshToken = result.RefreshToken
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("register/worker")]
        public async Task<ActionResult<LoginResponseDTO>> RegisterUser(RegisterDTO registerDto)
        {
            var result = await _authService.RegisterAsync(registerDto, Enums.Role.Worker);
            if (result.Error)
            {
                var response = new LoginResponseDTO
                {
                    Error = true,
                    ErrorMessage = result.ErrorMessage,
                    Token = ""
                };
                return StatusCode(500, response);
            }

            return Ok(new LoginResponseDTO
            {
                Error = false,
                Token = result.Token!,
                ErrorMessage = "",
                RefreshToken = result.RefreshToken
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDTO>> Login(LoginDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto);
            if (result.Error)
            {
                return Unauthorized(new LoginResponseDTO
                {
                    Error = true,
                    Token = "",
                    ErrorMessage = result.ErrorMessage,
                });
            }

            return Ok(new LoginResponseDTO
            {
                Error = false,
                ErrorMessage = "",
                Token = result.Token!,
                RefreshToken = result.RefreshToken
            });
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<RefreshTokenDto>> RefreshTokenGeneration(RefreshTokenReqestDto refreshToken)
        {
            var refreshTokenDto = await _authService.RefreshTokenHandler(refreshToken);

            return Ok(refreshTokenDto);
        }
    }
}
