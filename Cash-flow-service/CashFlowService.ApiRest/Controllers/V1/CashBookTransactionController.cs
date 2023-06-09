﻿using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Threading.Tasks;
using AutoMapper;
using CashFlowService.ApiRest.DTOs;
using CashFlowService.Core.DomainEntities;
using CashFlowService.Core.InputPorts;
using CashFlowService.Core.OutputPorts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CashFlowService.ApiRest.Controllers.V1;

[Route("api/cashflow/cashbooks")]
[ApiVersion("1", Deprecated = false)]
[ApiController]
public class CashBookTransactionController : ControllerBase
{
    private readonly ICashBookTransactionService _cashBookTransactionService;
    private readonly ILogger<CashBookTransactionController> _logger;
    private readonly IMapper _mapper;

    public CashBookTransactionController(
        ICashBookTransactionService cashBookTransactionService,
        ILogger<CashBookTransactionController> logger,
        IMapper mapper)
    {
        _cashBookTransactionService = cashBookTransactionService;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpPost("{id}/transactions", Name = "NewCashBookTransaction")]
    [ProducesResponseType(typeof(CashBookTransaction), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<CashBookTransaction>> Post(Guid id, [FromBody] CashBookTransactionDTO cashBookTransactionDTO)
    {
        try
        {
            if (id == Guid.Empty)
                return BadRequest("Invalid ID");

            var cashBookTransaction = _mapper.Map<CashBookTransaction>(cashBookTransactionDTO);
            cashBookTransaction.CashBookId = id;

            var createdTransaction = await _cashBookTransactionService.CreateNewCashBookTransactionAsync(cashBookTransaction);
            if (createdTransaction == null)
            {
                _logger.LogError("An error occurred while creating a new cash book transaction. Verify Cashbook ID.");
                return BadRequest("An error occurred while creating a new cash book transaction. Verify Cashbook ID."); ;
            }
            var createdTransactionDTO = _mapper.Map<CashBookTransactionDTO>(createdTransaction);

            return CreatedAtRoute("NewCashBookTransaction", new { id = createdTransaction.Id }, createdTransaction);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating a new cash book transaction.");
            return StatusCode(500, "An internal server error occurred.");
        }
    }


    [HttpGet("transactions/{transactionId}", Name = "GetTransactionById")]
    [ProducesResponseType(typeof(CashBookTransaction), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<CashBookTransaction>> Get(Guid transactionId)
    {
        try
        {
            if (transactionId == Guid.Empty)
                return BadRequest("Invalid ID");

            var transaction = await _cashBookTransactionService.GetCashBookTransactionByIdAsync(transactionId);
            if (transaction == null)
            {
                return NotFound($"No cash book transaction found with ID {transactionId}");
            }
            return transaction;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while getting cash book transaction with ID {transactionId}.");
            return StatusCode(500, "An internal server error occurred.");
        }
    }

    [HttpGet("transactions", Name = "GetAllTransactions")]
    [ProducesResponseType(typeof(IEnumerable<CashBookTransaction>), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<IEnumerable<CashBookTransaction>>> Get()
    {
        try
        {
            var transactions = await _cashBookTransactionService.GetAllCashBookTransactionAsync();
            if (transactions == null || !transactions.Any())
            {
                return NotFound("No cash book transactions found.");
            }
            return Ok(transactions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting all cash book transactions.");
            return StatusCode(500, "An internal server error occurred.");
        }
    }
}
