using CashFlowService.Core.DomainEntities;
using CashFlowService.Core.InputPorts;
using CashFlowService.Core.OutputPorts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CashBookTransactionService : ICashBookTransactionService
{
    private readonly ICashBookTransactionRepository _cashBookTransactionRepository;
    private readonly ICashBookRepository _cashBookRepository;
    private readonly ILogger<CashBookTransactionService> _logger;

    public CashBookTransactionService(ICashBookTransactionRepository cashBookTransactionRepository, ICashBookRepository cashBookRepository, ILogger<CashBookTransactionService> logger)
    {
        _cashBookTransactionRepository = cashBookTransactionRepository ?? throw new ArgumentNullException(nameof(cashBookTransactionRepository));
        _cashBookRepository = cashBookRepository ?? throw new ArgumentNullException(nameof(cashBookRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<CashBookTransaction> CreateNewCashBookTransactionAsync(CashBookTransaction cashBookTransaction)
    {
        if (cashBookTransaction == null)
        {
            throw new ArgumentNullException(nameof(cashBookTransaction));
        }

        var cashBook = await _cashBookRepository.ReadCashBookByIdAsync(cashBookTransaction.CashBookId);
        if (cashBook == null)
        {
            throw new ArgumentException($"Not exist a cash book with ID {cashBookTransaction.CashBookId}", nameof(cashBook));
        }

        cashBookTransaction.CashBook = cashBook;
        return await _cashBookTransactionRepository.CreateCashBookTransactionAsync(cashBookTransaction);
    }


    public async Task<CashBookTransaction> GetCashBookTransactionByIdAsync(Guid id)
    {
        try
        {
            return await _cashBookTransactionRepository.ReadCashBookTransactionAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occurred while getting a cash book Transaction.");
            throw;
        }
    }

    public async Task<IEnumerable<CashBookTransaction>> GetAllCashBookTransactionAsync()
    {
        try
        {
            return await _cashBookTransactionRepository.ListCashBookTransactionsAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occurred while getting all cash book Transactions.");
            throw;
        }
    }

    public async Task<IEnumerable<CashBookTransaction>> GetCashBookTransactionsByDateAsync(DateOnly dateOnly)
    {
        try
        {
            return await _cashBookTransactionRepository.ListCashBookTransactionsByDateAsync(dateOnly);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occurred while getting cash book Transactions.");
            throw;
        }
    }
}
