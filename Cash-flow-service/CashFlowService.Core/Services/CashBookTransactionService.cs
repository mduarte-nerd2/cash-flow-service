using CashFlowService.Core.DomainEntities;
using CashFlowService.Core.InputPorts;
using CashFlowService.Core.OutputPorts;
using Microsoft.Extensions.Logging;

public class CashBookTransactionService : ICashBookTransactionService
{
    private readonly ICashBookTransactionRepository _cashBookTransactionRepository;
    private readonly ICashBookRepository _cashBookRepository;
    private readonly ILogger<CashBookTransactionService> _logger;

    public CashBookTransactionService(ICashBookTransactionRepository cashBookTransactionRepository, ICashBookRepository cashBookRepository, ILogger<CashBookTransactionService> logger)
    {
        _cashBookTransactionRepository = cashBookTransactionRepository;
        _cashBookRepository = cashBookRepository;
        _logger = logger;
    }

    public async Task<CashBookTransaction> CreateNewCashBookTransactionAsync(CashBookTransaction cashBookTransaction)
    {
        try
        {
            // Verificar se o CashBook existe
            var cashBook = await _cashBookRepository.ReadCashBookByIdAsync(cashBookTransaction.CashBookId);
            if (cashBook == null)
            {
                throw new ArgumentException("Cash book not found.");
            }
            cashBookTransaction.CashBook = cashBook;
            return await _cashBookTransactionRepository.CreateCashBookTransactionAsync(cashBookTransaction);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occurred while creating a new cash book Transaction");
            throw;
        }
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
}
