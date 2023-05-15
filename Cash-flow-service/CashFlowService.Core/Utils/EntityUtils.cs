using System;
namespace CashFlowService.Core.Utils
{
	internal static class EntityUtils
	{
        internal static decimal RoundToTwoDecimalPlaces(decimal value)
        {
            return Math.Round(value, 2);
        }

    }
}

