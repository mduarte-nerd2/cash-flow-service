using System;
using CashFlowService.Core.DomainEntities;

namespace CashFlowService.Core.InputPorts
{
	public interface ICashBookManagerFacade
	{
        Task<CashBookTransactionsSummary> DailyTransactionsSummary(string dateOnly);
        Task<CashBookTransactionsSummary> CashBookTransactionSummary(Guid cashBookId);
    }
}

