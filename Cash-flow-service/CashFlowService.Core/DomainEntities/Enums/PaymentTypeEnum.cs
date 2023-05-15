using System;
namespace CashFlowService.Core.DomainEntities.Enums;

public enum PaymentTypeEnum : int
{
    Cash = 1,
    CreditCard = 2,
    DebitCard = 3,
    BankTransfer = 4,
    Pix = 5,
    Check = 6,
    Voucher = 7,
    BankSlips = 8,
    Withdrawal = 9,
    JudicialBlock = 10
}

