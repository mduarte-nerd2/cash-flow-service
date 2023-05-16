using System;
using System.Threading.Tasks;
using CashFlowService.Core.DomainEntities;
using CashFlowService.Core.InputPorts;
using CashFlowService.Core.Services;
using CashFlowService.Core.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CashFlowService.ApiRest.Controllers.V1
{
    [Route("api/cashflow/cashbooks/{id}/summary")]
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CashBookTransactionsSummary))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CashBookTransactionsSummary>> Get(Guid id)
        {
            try
            {
                var result = await _cashBookManagerFacade.CashBookTransactionSummary(id);

                if (result == null)
                {
                    _logger.LogInformation($"No transaction summary found for cash book with ID: {id}");
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving cash book transaction summary by ID");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving cash book transaction summary by ID");
            }
        }

        [HttpGet("by-date", Name = "GetCashBookTransactionSummaryByDate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CashBookTransactionsSummary))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CashBookTransactionsSummary>> Get(string dateOnly)
        {
            try
            {
                var result = await _cashBookManagerFacade.DailyTransactionsSummary(dateOnly);

                if (result == null)
                {
                    _logger.LogInformation($"No transaction summary found for date: {dateOnly}");
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving cash book transaction summary by date");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving cash book transaction summary by date");
            }
        }
    }
}
