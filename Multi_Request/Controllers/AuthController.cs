using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Multi_Request.Auth;
using Multi_Request.Dtos;
using Multi_Request.Models;
using Multi_Request.Services.PayloadFilterService;

namespace Multi_Request.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserAuthService _userAuthService;
        public AuthController(IUserAuthService userAuthService)
        {
            _userAuthService = userAuthService;
        }

        [HttpPost("Register")]
        [TypeFilter(typeof(MaxPayloadSizeFilterAttribute))]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegistrationDto request)
        {
            var response = await _userAuthService.Register(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpPost("Login")]
        public async Task<ActionResult<ServiceResponse<UserAuthenticatedDto>>> Login(UserLoginDto request)
        {
            var response = await _userAuthService.Login(request.Email, request.Password);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);

        }
    }
}
