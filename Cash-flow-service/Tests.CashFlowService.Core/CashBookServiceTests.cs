using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CashFlowService.Core.DomainEntities;
using CashFlowService.Core.InputPorts;
using CashFlowService.Core.OutputPorts;
using CashFlowService.Core.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Tests.CashFlowService.Core;

[TestFixture]
public class CashBookServiceTests
{
    private Mock<ICashBookRepository> _mockCashBookRepository;
    private Mock<ILogger<CashBookService>> _mockLogger;
    private CashBookService _cashBookService;

    [SetUp]
    public void Setup()
    {
        _mockCashBookRepository = new Mock<ICashBookRepository>();
        _mockLogger = new Mock<ILogger<CashBookService>>();
        _cashBookService = new CashBookService(_mockCashBookRepository.Object, _mockLogger.Object);
    }

    [Test]
    public async Task CreateNewCashBookAsync_And_ValidCashBook_Returns()
    {
        // Arrange
        var cashBook = new CashBook(11.71m);

        _mockCashBookRepository
            .Setup(repository => repository.CreateCashBookAsync(cashBook))
            .ReturnsAsync(cashBook);

        // Act
        var result = await _cashBookService.CreateNewCashBookAsync(cashBook);

        // Assert
        Assert.AreEqual(cashBook, result);
    }

    [Test]
    public async Task GetCashBookByDateAsync_ValidDate_ReturnsCashBook()
    {
        // Arrange
        var dateOnly = "2023-05-15";
        var cashBook = new CashBook(11.71m);

        _mockCashBookRepository
            .Setup(repository => repository.ReadCashBookAsync(dateOnly))
            .ReturnsAsync(cashBook);

        // Act
        var result = await _cashBookService.GetCashBookByDateAsync(dateOnly);

        // Assert
        Assert.AreEqual(cashBook, result);
    }
}
