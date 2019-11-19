using AutoMapper;
using WebApi.Domain.DTO;
using WebApi.Domain.Entities;

namespace WebApi.Domain.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();
        }
    }
}