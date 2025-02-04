using AutoMapper;
using Wallet.Domain.Entities.Model;
using Wallet.SharedKernel.DataTransferObjects;

namespace Wallet.API.Finance;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Transaction, TransactionReadDto>();
        CreateMap<TransactionCreateDto, Transaction>();
        CreateMap<IEnumerator<TransactionReadDto>, IEnumerator<Transaction>>();
        CreateMap<LocationDto, Location>();
        CreateMap<TransactionPublishedDto, TransactionCreateDto>();

        CreateMap<Account, AccountReadDto>();
        CreateMap<AccountCreateDto, Account>();
    }
}