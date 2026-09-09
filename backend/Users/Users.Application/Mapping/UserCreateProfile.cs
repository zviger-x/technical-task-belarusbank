using AutoMapper;
using Users.Application.Contracts;
using Users.Domain;

namespace Users.Application.Mapping
{
    public class UserCreateProfile : Profile
    {
        public UserCreateProfile()
        {
            CreateMap<CreateUserDto, User>();
            CreateMap<User, CreateUserDto>();
        }
    }
}
