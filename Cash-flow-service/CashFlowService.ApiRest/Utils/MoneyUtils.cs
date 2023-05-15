using System;
namespace CashFlowService.ApiRest.Utils
{
	public static class MoneyUtils
	{
        public static decimal RoundToTwoDecimalPlaces(decimal value)
        {
            return Math.Round(value, 2);
        }

    }
}

