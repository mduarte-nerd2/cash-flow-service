using System;
using System.Threading.Tasks;
using CashFlowService.Core.DomainEntities;
using CashFlowService.Core.InputPorts;
using CashFlowService.Core.Services;
using CashFlowService.Core.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CashFlowService.ApiRest.Controllers.V1;


[Route("api/cashflow/cashbooks/{id}/[controller]")]
[ApiVersion("1", Deprecated = false)]
[ApiController]
public class TransactionSummaryController : ControllerBase
{
    private readonly ICashBookManagerFacade _cashBookManagerFacade;
    private readonly ILogger<TransactionSummaryController> _logger;

    public TransactionSummaryController(ICashBookManagerFacade cashBookManagerFacade, ILogger<TransactionSummaryController> logger)
    {
        _cashBookManagerFacade = cashBookManagerFacade;
        _logger = logger;
    }

    [HttpGet("by-id", Name = "GetCashBookTransactionSummary")]
    public async Task<ActionResult<CashBookTransactionsSummary>> GetCashBookTransactionSummary(Guid id)
    {
        var result = await _cashBookManagerFacade.CashBookTransactionSummary(id);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("by-date", Name = "GetCashBookTransactionSummaryByDate")]
    public async Task<ActionResult<CashBookTransactionsSummary>> GetCashBookTransactionSummaryByDate(string dateOnly)
    {
        var result = await _cashBookManagerFacade.DailyTransactionsSummary(dateOnly);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }
}
