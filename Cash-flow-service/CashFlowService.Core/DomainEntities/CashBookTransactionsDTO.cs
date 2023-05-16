using System;
namespace CashFlowService.Core.DomainEntities
{
	public class CashBookTransactionsSummary
	{
		public IList<CashBookTransaction> Credits { get; set; }
        public IList<CashBookTransaction> Debits { get; set; }
        public decimal TotalAmountCredits { get; set; }
        public decimal TotalAmountDebits { get; set; }
		public decimal DayBalance { get; set; }
        public decimal FinalBalance { get; set; }
        public decimal InitialBalance { get; set; }
        public int CountCreditsTransactions { get; set; }
        public int CountDebitTransactions { get; set; }
        public DateOnly DateOfTransactions { get; set; }
    }
}

