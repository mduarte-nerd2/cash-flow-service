using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CashFlowService.Core.DomainEntities;

namespace CashFlowService.Core.OutputPorts
{
	public interface ICashBookRepository
	{
        Task<CashBook> CreateCashBookAsync(CashBook cashBook);

        Task<CashBook> ReadCashBookAsync(string dateOnly);

        Task<IEnumerable<CashBook>> ListCashBookAsync();

        Task<bool> UpdateCashBookAsync(CashBook cashBook);

        Task<bool> DeleteCashBookAsync(Guid id);

        Task<CashBook> ReadCashBookByIdAsync(Guid id);
    }
}

