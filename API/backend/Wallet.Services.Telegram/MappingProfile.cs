using System.Transactions;
using AutoMapper;
using Wallet.Services.Telegram.Dtos;

namespace Wallet.Services.Telegram;

public class MappingProfile : Profile {
    public MappingProfile() {
        CreateMap<Transaction, TransactionReadDto>();
        CreateMap<TransactionCreateDto, Transaction>();
        CreateMap<TransactionReadDto, TransactionPublishedDto>();
    }
}