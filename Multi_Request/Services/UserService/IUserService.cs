using Multi_Request.Dtos;
using Multi_Request.Models;

namespace Multi_Request.Services.UserService
{
    public interface IUserService
    {
        Task<ServiceResponse<List<GetUserDto>>> GetAllUser();
    }
}
