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

[Route("api/cashflow/[controller]")]
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
        var response = await _cashBookService.GetCashBookByDateAsync(dateOnly);
        if (response == null)
        {
            return NotFound($"No cash book found for date {dateOnly}");
        }
        return Ok(response);
    }

    [HttpGet(Name = "GetAll")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CashBook>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<CashBook>>> Get()
    {
        var response = await _cashBookService.GetAllCashBook();
        if (response == null || !response.Any())
        {
            return NotFound("No cash books found");
        }
        return Ok(response.ToList());
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult> Post([FromBody] CashBook value)
    {
        await _cashBookService.CreateNewCashBookAsync(value);
        return CreatedAtRoute("Get", new { dateOnly = value.Date.ToString("yyyy-MM-dd") }, value);
    }
}
