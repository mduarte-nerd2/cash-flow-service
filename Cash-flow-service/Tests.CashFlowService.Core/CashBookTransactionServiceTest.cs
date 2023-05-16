using System;
using CashFlowService.Core.DomainEntities;
using CashFlowService.Core.OutputPorts;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests.CashFlowService.Core;

[TestFixture]
public class CashBookTransactionServiceTests
{
    private Mock<ICashBookTransactionRepository> _mockCashBookTransactionRepository;
    private Mock<ICashBookRepository> _mockCashBookRepository;
    private Mock<ILogger<CashBookTransactionService>> _mockLogger;
    private CashBookTransactionService _cashBookTransactionService;

    [SetUp]
    public void Setup()
    {
        _mockCashBookTransactionRepository = new Mock<ICashBookTransactionRepository>();
        _mockCashBookRepository = new Mock<ICashBookRepository>();
        _mockLogger = new Mock<ILogger<CashBookTransactionService>>();
        _cashBookTransactionService = new CashBookTransactionService(
            _mockCashBookTransactionRepository.Object,
            _mockCashBookRepository.Object,
            _mockLogger.Object);
    }


    [Test]
    public async Task CreateNewCashBookTransactionAsync_ValidInput_ReturnsCashBookTransaction()
    {
        // Arrange
        var cashBookId = Guid.NewGuid();
        var cashBookTransaction = new CashBookTransaction
        {
            CashBookId = cashBookId
        };

        var cashBook = new CashBook(171.12m);
        _mockCashBookRepository
            .Setup(repository => repository.ReadCashBookByIdAsync(cashBookId))
            .ReturnsAsync(cashBook);

        _mockCashBookTransactionRepository
            .Setup(repository => repository.CreateCashBookTransactionAsync(cashBookTransaction))
            .ReturnsAsync(cashBookTransaction);

        // Act
        var result = await _cashBookTransactionService.CreateNewCashBookTransactionAsync(cashBookTransaction);

        // Assert
        Assert.AreEqual(cashBookTransaction, result);
        Assert.AreEqual(cashBook, cashBookTransaction.CashBook);
        _mockCashBookRepository.Verify(repository => repository.ReadCashBookByIdAsync(cashBookId), Times.Once);
        _mockCashBookTransactionRepository.Verify(repository => repository.CreateCashBookTransactionAsync(cashBookTransaction), Times.Once);
    }


    [Test]
    public void CreateNewCashBookTransactionAsync_NullInput_ThrowsArgumentNullException()
    {
        CashBookTransaction cashBookTransaction = null;

        Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            await _cashBookTransactionService.CreateNewCashBookTransactionAsync(cashBookTransaction);
        });
    }


    [Test]
    public void CreateNewCashBookTransactionAsync_InvalidCashBookId_ThrowsArgumentException()
    {
        var cashBookId = Guid.NewGuid();
        var cashBookTransaction = new CashBookTransaction
        {
            CashBookId = cashBookId
        };

        _mockCashBookRepository.Setup(repository => repository.ReadCashBookByIdAsync(cashBookId))
            .ReturnsAsync((CashBook)null);

        Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await _cashBookTransactionService.CreateNewCashBookTransactionAsync(cashBookTransaction);
        });
    }


    [Test]
    public async Task GetCashBookTransactionByIdAsync_ValidId_ReturnsCashBookTransaction()
    {
        var id = Guid.NewGuid();
        var cashBookTransaction = new CashBookTransaction();

        _mockCashBookTransactionRepository
            .Setup(repository => repository.ReadCashBookTransactionAsync(id))
            .ReturnsAsync(cashBookTransaction);

        var result = await _cashBookTransactionService.GetCashBookTransactionByIdAsync(id);

        Assert.AreEqual(cashBookTransaction, result);
        _mockCashBookTransactionRepository.Verify(repository => repository.ReadCashBookTransactionAsync(id), Times.Once);
    }
}

