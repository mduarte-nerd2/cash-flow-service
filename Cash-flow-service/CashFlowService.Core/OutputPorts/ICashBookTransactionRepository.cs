using System;
using CashFlowService.Core.DomainEntities;

namespace CashFlowService.Core.OutputPorts
{
	public interface ICashBookTransactionRepository
	{
        Task<CashBookTransaction> CreateCashBookTransactionAsync(CashBookTransaction cashBookTransaction);

        Task<CashBookTransaction> ReadCashBookTransactionAsync(Guid id);

        Task<IEnumerable<CashBookTransaction>> ListCashBookTransactionsAsync();

        Task<IEnumerable<CashBookTransaction>> ListCashBookTransactionsByDateAsync(DateOnly date);
    }
}

