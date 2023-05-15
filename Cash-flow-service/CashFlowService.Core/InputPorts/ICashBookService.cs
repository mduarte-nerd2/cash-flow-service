using System;
using CashFlowService.Core.DomainEntities;

namespace CashFlowService.Core.InputPorts;

public interface ICashBookService
{
    Task<CashBook> CreateNewCashBookAsync(CashBook cashBook);

    Task<CashBook> ReadCashBookByIdAsync(Guid id);

    Task<CashBook> GetCashBookByDateAsync(string dateOnly);

    Task<IEnumerable<CashBook>> GetAllCashBook();

    Task<bool> RemoveCashBookAsync(Guid id);

    Task<bool> AlterCashBookAsync(Guid id, CashBook value);
}