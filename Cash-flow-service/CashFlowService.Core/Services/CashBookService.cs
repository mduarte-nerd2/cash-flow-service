using System;
using System.Linq;
using System.Threading.Tasks;
using CashFlowService.Core.DomainEntities;
using CashFlowService.Core.InputPorts;
using CashFlowService.Core.OutputPorts;
using CashFlowService.Core.Validators;
using Microsoft.Extensions.Logging;

namespace CashFlowService.Core.Services;

public class CashBookService : ICashBookService
{
    private readonly ICashBookRepository _cashBookRepository;
    private readonly ILogger<CashBookService> _logger;

    public CashBookService(ICashBookRepository cashBookRepository, ILogger<CashBookService> logger)
    {
        _cashBookRepository = cashBookRepository ?? throw new ArgumentNullException(nameof(cashBookRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<CashBook> CreateNewCashBookAsync(CashBook cashBook)
    {
        try
        {
            return await _cashBookRepository.CreateCashBookAsync(cashBook);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occurred while creating a new cash book");
            throw;
        }
    }

    public async Task<CashBook> GetCashBookByDateAsync(string dateOnly)
    {
        try
        {
            return await _cashBookRepository.ReadCashBookAsync(dateOnly);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occurred while getting a cash book by date");
            throw;
        }
    }

    public async Task<bool> CloseCashBookAsync(string dateOnly)
    {
        try
        {
            var cashBook = await _cashBookRepository.ReadCashBookAsync(dateOnly);

            if (cashBook == null)
            {
                _logger.LogWarning("No cash book was found for the provided date");
                return false;
            }

            if (cashBook.IsClosed)
            {
                _logger.LogWarning("Cash book is already closed");
                return false;
            }

            var validator = new CashBookValidator();
            var validationResult = validator.Validate(cashBook);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning($"Cash book of day {cashBook.Date} cannot be closed because it's not ready.");
                return false;
            }

            if (cashBook.IsClosed)
            {
                _logger.LogWarning($"Cash book of day {cashBook.Date} is already closed.");
                return false;
            }

            cashBook.IsClosed = true;
            cashBook.DayBalance = CalculateDayBalance(cashBook);
            await _cashBookRepository.UpdateCashBookAsync(cashBook);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occurred while closing a cash book.");
            throw;
        }
    }

    private decimal CalculateDayBalance(CashBook cashBook)
    {
        //decimal dayBalance = cashBook.InitialBalance;

        //foreach (var transaction in cashBook.TransactionList)
        //{
        //    dayBalance += transaction.TransactionType switch
        //    {
        //        DomainEntities.Enums.TransactionTypeEnum.Credit => transaction.Amount,
        //        DomainEntities.Enums.TransactionTypeEnum.Debit => -transaction.Amount,
        //        _ => 0
        //    };
        //}

        return cashBook.DayBalance;
    }

    public async Task<IEnumerable<CashBook>> GetAllCashBook()
    {
        try
        {
            return await _cashBookRepository.ListCashBookAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occurred while getting all cash books.");
            throw;
        }
    }

    public async Task<bool> RemoveCashBookAsync(Guid id)
    {
        try
        {
            var cashBook = await _cashBookRepository.ReadCashBookByIdAsync(id);

            if (cashBook == null)
            {
                _logger.LogWarning($"No cash book was found with the provided id: {id}");
                return false;
            }

            await _cashBookRepository.DeleteCashBookAsync(cashBook.Id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An exception occurred while removing cash book with id: {id}");
            throw;
        }
    }

    public async Task<bool> AlterCashBookAsync(Guid id, CashBook value)
    {
        try
        {
            var existingCashBook = await _cashBookRepository.ReadCashBookByIdAsync(id);

            if (existingCashBook == null)
            {
                _logger.LogWarning($"No cash book was found with the provided id: {id}");
                return false;
            }

            existingCashBook.SetNewInitialBalance(value.InitialBalance);
            existingCashBook.IsClosed = value.IsClosed;

            var validator = new CashBookValidator();
            var validationResult = validator.Validate(existingCashBook);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning($"Cash book with id {id} cannot be updated because it's not valid.");
                return false;
            }

            await _cashBookRepository.UpdateCashBookAsync(existingCashBook);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An exception occurred while altering cash book with id: {id}");
            throw;
        }
    }
}
