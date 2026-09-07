using AutoMapper;
using Users.Application.Contracts;
using Users.Domain;

namespace Users.Application.Mapping
{
    public class UserDtoProfile : Profile
    {
        public UserDtoProfile()
        {
            CreateMap<UserDto, User>();
            CreateMap<User, UserDto>();
        }
    }
}
