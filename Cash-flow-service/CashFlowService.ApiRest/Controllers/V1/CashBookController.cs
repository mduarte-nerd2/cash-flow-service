using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CashFlowService.Core.InputPorts;
using CashFlowService.Core.DomainEntities;

namespace CashFlowService.ApiRest.Controllers.V1;

[Route("api/cashflow/cashbooks")]
[ApiVersion("1", Deprecated = false)]
[ApiController]
public class CashBookController : ControllerBase
{
    private readonly ICashBookService _cashBookService;
    private readonly ILogger<CashBookController> _logger;

    public CashBookController(ICashBookService cashBookService, ILogger<CashBookController> logger)
    {
        _cashBookService = cashBookService;
        _logger = logger;
    }

    [HttpGet("{dateOnly}", Name = "Get")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CashBook))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CashBook>> Get(string dateOnly)
    {
        try
        {
            var response = await _cashBookService.GetCashBookByDateAsync(dateOnly);
            if (response == null)
            {
                _logger.LogInformation($"No cash book found for date {dateOnly}");
                return NotFound($"No cash book found for date {dateOnly}");
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving cash book by date");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving cash book by date");
        }
    }


    [HttpGet(Name = "GetAll")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CashBook>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<CashBook>>> Get()
    {
        try
        {
            var response = await _cashBookService.GetAllCashBook();
            if (response == null || !response.Any())
            {
                _logger.LogInformation("No cash books found");
                return NotFound("No cash books found");
            }
            return Ok(response.ToList());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving all cash books");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving all cash books");
        }
    }


    [HttpPost(Name = "Post")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult> Post([FromBody] decimal initialBalance)
    {
        try
        {
            var newCashBook = new CashBook(Utils.MoneyUtils.RoundToTwoDecimalPlaces(initialBalance));
            await _cashBookService.CreateNewCashBookAsync(newCashBook);
            _logger.LogInformation("Cash book created successfully");
            return CreatedAtRoute("Get", new { dateOnly = newCashBook.Date.ToString("yyyy-MM-dd") }, newCashBook);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating a new cash book");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating a new cash book");
        }
    }
}
