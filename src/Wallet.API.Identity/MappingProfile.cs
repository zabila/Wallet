using AutoMapper;
using Wallet.Domain.Entities.Model;
using Wallet.Shared.DataTransferObjects;

namespace Wallet.API.Identity;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterUserDto, User>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));;
    }
}