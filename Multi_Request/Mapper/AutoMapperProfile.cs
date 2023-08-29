using AutoMapper;
using Multi_Request.Dtos;
using Multi_Request.Models;

namespace Multi_Request.Mapper
{
    public class AutoMapperProfile : Profile

    {
        public AutoMapperProfile()
        {
            CreateMap<User, GetUserDto>();
        }
    }
}
