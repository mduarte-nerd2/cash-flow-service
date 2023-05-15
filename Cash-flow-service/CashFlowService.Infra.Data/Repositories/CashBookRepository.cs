using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CashFlowService.Core.DomainEntities;
using CashFlowService.Core.OutputPorts;
using CashFlowService.Infra.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CashFlowService.Infra.Data.Repositories;

public class CashBookRepository : ICashBookRepository
{
    private readonly InMemoryContext _dbContext;
    private readonly ILogger<CashBookRepository> _logger;

    public CashBookRepository(InMemoryContext dbContext, ILogger<CashBookRepository> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<CashBook> CreateCashBookAsync(CashBook cashBook)
    {
        try
        {
            var cashBookEntry = await _dbContext.AddAsync(cashBook);
            await _dbContext.SaveChangesAsync();
            return cashBookEntry.Entity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create a cash book.");
            throw;
        }
    }

    public async Task<CashBook> ReadCashBookAsync(string dateOnly)
    {
        try
        {
            var date = DateOnly.Parse(dateOnly);

            var cashBook = await _dbContext.Set<CashBook>().FirstOrDefaultAsync(c => c.Date == date);
            return cashBook;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to read a cash book.");
            throw;
        }
    }

    public async Task<bool> UpdateCashBookAsync(CashBook cashBook)
    {
        try
        {
            _dbContext.Update(cashBook);
            var count = await _dbContext.SaveChangesAsync();
            return count > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update a cash book.");
            throw;
        }
    }

    public async Task<IEnumerable<CashBook>> ListCashBookAsync()
    {
        try
        {
            var cashBooks = await _dbContext.Set<CashBook>().ToListAsync();
            return cashBooks.AsEnumerable();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to list cash books.");
            throw;
        }
    }

    public async Task<bool> DeleteCashBookAsync(Guid id)
    {
        try
        {
            var cashBook = await _dbContext.Set<CashBook>().FindAsync(id);
            if (cashBook != null)
            {
                _dbContext.Remove(cashBook);
                var count = await _dbContext.SaveChangesAsync();
                return count > 0;
            }
            else
            {
                _logger.LogWarning($"Cash book with ID {id} not found.");
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to delete a cash book with ID {id}.");
            throw;
        }
    }

    public async Task<CashBook> ReadCashBookByIdAsync(Guid id)
    {
        try
        {
            var cashBook = await _dbContext.Set<CashBook>().FirstOrDefaultAsync(c => c.Id == id);
            return cashBook;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to read a cash book with ID {id}.");
            throw;
        }
    }
}
