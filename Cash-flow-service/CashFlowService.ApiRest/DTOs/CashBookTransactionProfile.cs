using System;
using AutoMapper;
using CashFlowService.ApiRest.DTOs;
using CashFlowService.Core.DomainEntities;
using CashFlowService.Core.DomainEntities.Enums;

namespace CashFlowService.ApiRest.MappingProfiles;

public class EnumToStringConverter<TEnum> : IValueConverter<TEnum, string>
    where TEnum : Enum
{
    public string Convert(TEnum sourceMember, ResolutionContext context)
    {
        return sourceMember.ToString();
    }
}

public class CashBookTransactionProfile : Profile
{
    public CashBookTransactionProfile()
    {
        CreateMap<CashBookTransaction, CashBookTransactionDTO>()
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
            .ForMember(dest => dest.PaymentType, opt => opt.ConvertUsing(new EnumToStringConverter<PaymentTypeEnum>(), src => src.PaymentType))
            .ForMember(dest => dest.TransactionType, opt => opt.ConvertUsing(new EnumToStringConverter<TransactionTypeEnum>(), src => src.TransactionType));

        CreateMap<CashBookTransactionDTO, CashBookTransaction>()
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
            .ForMember(dest => dest.PaymentType, opt => opt.MapFrom(src => src.PaymentType))
            .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src => src.TransactionType))
            .ForMember(dest => dest.CashBookId, opt => opt.Ignore())
            .ForMember(dest => dest.CashBook, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
    }
}
