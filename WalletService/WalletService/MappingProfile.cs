using AutoMapper;
using Entities.Model;
using Shared.DataTransferObjects;
using User = Entities.Model.User;

namespace WalletService;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Transaction, TransactionReadDto>();
        CreateMap<TransactionCreateDto, Transaction>();
        CreateMap<IEnumerator<TransactionReadDto>, IEnumerator<Transaction>>();

        CreateMap<Account, AccountReadDto>();
        CreateMap<AccountCreateDto, Account>();
    }
}