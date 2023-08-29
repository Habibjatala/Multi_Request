using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Multi_Request.Dtos;
using Multi_Request.Models;
using Multi_Request.Services.UserService;

namespace Multi_Request.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }
        [Authorize]
        [HttpGet]
        public async Task<ServiceResponse<List<GetUserDto>>> GetAllUser()
        {
            string clientIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            _logger.LogInformation("Request IP Add @ip", clientIpAddress);
            return await _userService.GetAllUser();
        }

    }
}
