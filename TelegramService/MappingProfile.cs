using System.Transactions;
using AutoMapper;
using TelegramService.Dtos;

namespace TelegramService;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Transaction, TransactionReadDto>();
        CreateMap<TransactionCreateDto, Transaction>();
        CreateMap<TransactionReadDto, TransactionPublishedDto>();
    }
}