using AutoMapper;
using Entities.Model;
using Shared.DataTransferObjects;
using User = Entities.Model.User;

namespace WalletService;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Transaction, TransactionDto>();
        CreateMap<TransactionForCreationDto, Transaction>();

        CreateMap<UserForRegistrationDto, User>();
    }
}