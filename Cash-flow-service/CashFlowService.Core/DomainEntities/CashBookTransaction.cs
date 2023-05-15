using System;
using CashFlowService.Core.Utils;

namespace CashFlowService.Core.DomainEntities;

public class CashBookTransaction : EntityBase
{
    private string description;
    private decimal amount = 0.00M;
    //private CashBook cashBook;

    public CashBookTransaction() : base() { }

    public CashBookTransaction(
        CashBook cashBook,
        string description,
        decimal amount,
        Enums.PaymentTypeEnum paymentType,
        Enums.TransactionTypeEnum transactionType
        ) : base()
    {
        //this.cashBook = cashBook ?? throw new ArgumentNullException(nameof(cashBook));
        this.description = description;
        this.amount = EntityUtils.RoundToTwoDecimalPlaces(amount);
        this.PaymentType = paymentType;
        this.TransactionType = transactionType;
    }

    public Guid CashBookId { get; set; }

    public CashBook CashBook { get; set; }

    public string Description { get => description; private set => description = value; }

    public decimal Amount { get => amount; private set => amount = EntityUtils.RoundToTwoDecimalPlaces(value); }

    public Enums.PaymentTypeEnum PaymentType { get; private set; }

    public Enums.TransactionTypeEnum TransactionType { get; private set; }

}

