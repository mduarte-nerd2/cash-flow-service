using System;
using CashFlowService.Core.BusinessRulesDecorators;
using CashFlowService.Core.DomainEntities;

namespace CashFlowService.Core.BusinessRulesDecorators
{
	public class CashBookWithOnSameDayValidation : ICashBookBusinessRule
	{
		public CashBookWithOnSameDayValidation()
		{
		}

        public Task<dynamic> ValidateBusinessRule(CashBook cashBook)
        {
            throw new NotImplementedException();
        }
    }
}

