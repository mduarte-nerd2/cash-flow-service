using System;
using CashFlowService.Core.DomainEntities;
using FluentValidation;

namespace CashFlowService.Core.Validators
{
	public class CashBookValidator : AbstractValidator<CashBook>
	{
		public CashBookValidator()
		{
			RuleFor(cb => cb.DayBalance).NotNull().NotEmpty().WithMessage("CashBook Balance can`t be empty or null.");

        }
    }
}

