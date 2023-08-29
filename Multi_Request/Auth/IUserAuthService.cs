using Multi_Request.Dtos;
using Multi_Request.Models;

namespace Multi_Request.Auth
{
    public interface IUserAuthService
    {
        Task<ServiceResponse<int>> Register(UserRegistrationDto request);
        Task<ServiceResponse<UserAuthenticatedDto>> Login(String username, string password);
        Task<bool> UserExists(string username);
    }
}
