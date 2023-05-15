using System;
using CashFlowService.Core.DomainEntities;

namespace CashFlowService.Core.BusinessRulesDecorators
{
	public class BaseBusinessRule : ICashBookBusinessRule
	{
		public BaseBusinessRule()
		{
		}

        public Task<dynamic> ValidateBusinessRule(CashBook cashBook)
        {
            throw new NotImplementedException();
        }
    }
}

