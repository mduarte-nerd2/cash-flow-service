using System;
using CashFlowService.Core.DomainEntities;

namespace CashFlowService.Core.BusinessRulesDecorators
{
	public interface ICashBookBusinessRule
	{
		Task<dynamic> ValidateBusinessRule(CashBook cashBook);
	}
}
