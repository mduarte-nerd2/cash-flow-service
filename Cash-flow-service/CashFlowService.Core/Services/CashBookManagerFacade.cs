using System;
using System.Transactions;
using CashFlowService.Core.DomainEntities;
using CashFlowService.Core.DomainEntities.Enums;
using CashFlowService.Core.InputPorts;
using CashFlowService.Core.OutputPorts;
using CashFlowService.Core.Validators;
using Microsoft.Extensions.Logging;

namespace CashFlowService.Core.Services;

public class CashBookManagerFacade : ICashBookManagerFacade
{
    private readonly ICashBookService _cashBookService;
    private readonly ICashBookTransactionService _cashBookTransactionService;
    private readonly ILogger<CashBookManagerFacade> _logger;

    public CashBookManagerFacade(ICashBookService cashBookService, ICashBookTransactionService cashBookTransactionService, ILogger<CashBookManagerFacade> logger)
    {
        _cashBookService = cashBookService;
        _cashBookTransactionService = cashBookTransactionService;
        _logger = logger;
    }

    public async Task<CashBookTransactionsSummary> CashBookTransactionSummary(Guid cashBookId)
    {
        try
        {
            var cashBook = await _cashBookService.ReadCashBookByIdAsync(cashBookId);

            if (cashBook == null)
            {
                _logger.LogInformation("Cash book not found for the current day.");
                return (null);
            }
            if (cashBook.IsClosed)
            {
                _logger.LogWarning("Cash book is already closed");
                return (null);
            }

            CashBookTransactionsSummary resumeCashBookTransactions = await TransactionsSum(cashBook);

            _logger.LogInformation($"Cash book with id {cashBook.Id} of day {cashBook.Date} calculated successfully.");

            return resumeCashBookTransactions;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occurred while closing the cash book.");
            throw;
        }
    }


    public async Task<CashBookTransactionsSummary> DailyTransactionsSummary(string dateOnly)
    {
        try
        {
            var cashBook = await _cashBookService.GetCashBookByDateAsync(dateOnly);

            if (cashBook == null)
            {
                _logger.LogInformation("Cash book not found for the current day.");
                return (null);
            }
            if (cashBook.IsClosed)
            {
                _logger.LogWarning("Cash book is already closed");
                return (null);
            }

            CashBookTransactionsSummary resumeCashBookTransactions = await TransactionsSum(cashBook);

            _logger.LogInformation($"Cash book with id {cashBook.Id} of day {cashBook.Date} calculated successfully.");
            
            return resumeCashBookTransactions;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occurred while closing the cash book.");
            throw;
        }
    }

    private async Task<CashBookTransactionsSummary> TransactionsSum(CashBook cashBook)
    {
        var transactions = await _cashBookTransactionService.GetCashBookTransactionsByDateAsync(cashBook.Date);

        var resumeCashBookTransactions = new CashBookTransactionsSummary();
        resumeCashBookTransactions.Credits = new List<CashBookTransaction>();
        resumeCashBookTransactions.Debits = new List<CashBookTransaction>();

        var totalCredit = 0M;
        var totalDebit = 0M;
        var countCreditsTransactions = 0;
        var countDebitsTransactions = 0;


        foreach (var transaction in transactions)
        {
            if (transaction.TransactionType == TransactionTypeEnum.Credit)
            {
                totalCredit += transaction.Amount;
                resumeCashBookTransactions.Credits.Add(transaction);
            }
            else if (transaction.TransactionType == TransactionTypeEnum.Debit)
            {
                totalDebit += transaction.Amount;
                resumeCashBookTransactions.Debits.Add(transaction);
            }
            else
            {
                throw new ApplicationException("Transaction Type cannot be empty");
            }
        }
        countCreditsTransactions = resumeCashBookTransactions.Credits.Count;
        countDebitsTransactions = resumeCashBookTransactions.Debits.Count;

        resumeCashBookTransactions.DayBalance = cashBook.DayBalance;

        cashBook.DayBalance = totalCredit - totalDebit;
        cashBook.DayBalance += cashBook.InitialBalance;

        resumeCashBookTransactions.DayBalance = cashBook.DayBalance;
        resumeCashBookTransactions.CountCreditsTransactions = countCreditsTransactions;
        resumeCashBookTransactions.CountDebitTransactions = countDebitsTransactions;
        resumeCashBookTransactions.DateOfTransactions = cashBook.Date;

        resumeCashBookTransactions.TotalAmountCredits = totalCredit;
        resumeCashBookTransactions.TotalAmountDebits = totalDebit;
        resumeCashBookTransactions.InitialBalance = cashBook.InitialBalance;
        resumeCashBookTransactions.FinalBalance = cashBook.DayBalance;

        return resumeCashBookTransactions;
    }
}

