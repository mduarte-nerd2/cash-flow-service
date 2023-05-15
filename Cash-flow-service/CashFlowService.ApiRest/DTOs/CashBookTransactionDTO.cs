using System;
namespace CashFlowService.ApiRest.DTOs
{
	public class CashBookTransactionDTO
	{
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string PaymentType { get; set; }
        public string TransactionType { get; set; }
    }
}

