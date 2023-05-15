using System;
using CashFlowService.Core.Utils;

namespace CashFlowService.Core.DomainEntities;

public class CashBook : EntityBase
{
    private decimal initialBalance;

    public CashBook(decimal initialBalance) : base()
	{
        Date = new DateOnly(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day);
        DayOfYear = Date.DayOfYear;
        this.initialBalance = EntityUtils.RoundToTwoDecimalPlaces(initialBalance);
        IsClosed = false;
        IsDebitBalance = false;
        if (initialBalance < 0)
            IsDebitBalance = true;
        IsClosed = false;
    }

    public DateOnly Date { get; private set; }

    public int DayOfYear { get; set; }

    public decimal InitialBalance { get => initialBalance; private set => initialBalance = EntityUtils.RoundToTwoDecimalPlaces(value); }

    public decimal DayBalance { get; set; }

    public bool IsDebitBalance { get; private set; }

    public bool IsClosed { get; set; }

    public void SetNewInitialBalance(Decimal newValue)
    {
        InitialBalance = newValue;
    }
}