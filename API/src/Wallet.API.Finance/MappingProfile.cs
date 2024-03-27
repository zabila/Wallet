using AutoMapper;
using Wallet.Shared.DataTransferObjects;
using Wallet.Domain.Entities.Model;

namespace Wallet.API.Finance;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Transaction, TransactionReadDto>();
        CreateMap<TransactionCreateDto, Transaction>();
        CreateMap<IEnumerator<TransactionReadDto>, IEnumerator<Transaction>>();
        CreateMap<TransactionPublishedDto, TransactionCreateDto>();

        CreateMap<Account, AccountReadDto>();
        CreateMap<AccountCreateDto, Account>();
    }
}