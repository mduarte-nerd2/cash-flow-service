using System;
using CashFlowService.Core.DomainEntities;

namespace CashFlowService.Core.InputPorts;

public interface ICashBookTransactionService
{
    Task<CashBookTransaction> CreateNewCashBookTransactionAsync(CashBookTransaction cashBookTransaction);
    Task<IEnumerable<CashBookTransaction>> GetAllCashBookTransactionAsync();
    Task<CashBookTransaction> GetCashBookTransactionByIdAsync(Guid id);
    Task<IEnumerable<CashBookTransaction>> GetCashBookTransactionsByDateAsync(DateOnly dateOnly);
}

