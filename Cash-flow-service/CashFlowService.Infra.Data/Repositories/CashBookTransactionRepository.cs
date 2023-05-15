using System;
using System.Threading.Tasks;
using CashFlowService.Core.DomainEntities;
using CashFlowService.Core.OutputPorts;
using CashFlowService.Infra.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CashFlowService.Infra.Data.Repositories;

public class CashBookTransactionRepository : ICashBookTransactionRepository
{
    private readonly InMemoryContext _dbContext;
    private readonly ILogger<CashBookTransactionRepository> _logger;

    public CashBookTransactionRepository(
        InMemoryContext dbContext,
        ILogger<CashBookTransactionRepository> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<CashBookTransaction> CreateCashBookTransactionAsync(CashBookTransaction cashBookTransaction)
    {
        try
        {
            var cashBookTransactionEntry = await _dbContext.AddAsync(cashBookTransaction);
            await _dbContext.SaveChangesAsync();
            return cashBookTransactionEntry.Entity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occurred while creating a cash book transaction.");
            throw;
        }
    }

    public async Task<CashBookTransaction> ReadCashBookTransactionAsync(Guid id)
    {
        try
        {
            return await _dbContext.Set<CashBookTransaction>().FirstOrDefaultAsync(c => c.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occurred while reading a cash book transaction.");
            throw;
        }
    }

    public async Task<IEnumerable<CashBookTransaction>> ListCashBookTransactionsAsync()
    {
        try
        {
            var cashBoolTransactions = await _dbContext.Set<CashBookTransaction>().ToListAsync();
            return cashBoolTransactions.AsEnumerable();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occurred while getting all cash book transactions.");
            throw;
        }
    }

    public async Task<IEnumerable<CashBookTransaction>> ListCashBookTransactionsByDateAsync(DateOnly date)
    {
        try
        {
            var cashBookTransactions = await _dbContext.Set<CashBookTransaction>()
                .Where(transaction => transaction.CashBook.Date == date)
                .ToListAsync();

            return cashBookTransactions.AsEnumerable();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occurred while getting cash book transactions by date.");
            throw;
        }
    }


}
