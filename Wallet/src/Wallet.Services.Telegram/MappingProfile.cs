using AutoMapper;
using Wallet.Domain.Entities.Model;
using Wallet.Shared.DataTransferObjects;

namespace Wallet.Services.Telegram;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Transaction, TransactionReadDto>();
        CreateMap<Location, LocationDto>();
        CreateMap<TransactionCreateDto, Transaction>();
        CreateMap<TransactionReadDto, TransactionPublishedDto>();
    }
}