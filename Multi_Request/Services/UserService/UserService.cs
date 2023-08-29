using AutoMapper;
using Multi_Request.Dtos;
using Multi_Request.Models;
using Multi_Request.Services.Repository;

namespace Multi_Request.Services.UserService
{
    public class UserService:IUserService
    {
        private readonly MyRepository<User> _repository;
        private readonly IMapper _mapper;

        public UserService(MyRepository<User> repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<GetUserDto>>> GetAllUser()
        {
            var serviceReseponse = new ServiceResponse<List<GetUserDto>>();

            var user= await _repository.GetAll();

            serviceReseponse.Data= user.Data.Select(c=> _mapper.Map<GetUserDto>(c)).ToList();
            return serviceReseponse;


           
        }
    }
}
