using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using ChuBank.Application.Services;
using ChuBank.Domain.Interfaces;
using ChuBank.Application.DTOs;
using ChuBank.Domain.Entities;

namespace ChuBank.Tests.Application;

public class TransferServiceTests
{
    private readonly Mock<IAccountRepository> _accountRepositoryMock;
    private readonly Mock<IHolidayService> _holidayServiceMock;
    private readonly TransferService _transferService;

    public TransferServiceTests()
    {
        _accountRepositoryMock = new Mock<IAccountRepository>();
        _holidayServiceMock = new Mock<IHolidayService>();

        _transferService = new TransferService(_accountRepositoryMock.Object, _holidayServiceMock.Object);
    }

    [Fact]
    public async Task PerformTransfer_ShouldThrowException_WhenNotBusinessDay()
    {
        _holidayServiceMock.Setup(x => x.IsBusinessDayAsync(It.IsAny<DateTime>()))
                           .ReturnsAsync(false);

        var dto = new TransferDto(Guid.NewGuid(), Guid.NewGuid(), 100);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _transferService.PerformTransferAsync(dto));

        Assert.Equal("Transferências só podem ser feitas nos dias úteis.", exception.Message);

        _accountRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task PerformTransfer_ShouldSucceed_WhenBusinessDayAndBalanceSufficient()
    {
        _holidayServiceMock.Setup(x => x.IsBusinessDayAsync(It.IsAny<DateTime>()))
                           .ReturnsAsync(true);

        var originAccount = new Account("111", "Origin", "hash123");
        originAccount.Deposit(500); 
        var destAccount = new Account("222", "Destination", "hash123"); 

        _accountRepositoryMock.Setup(x => x.GetByIdAsync(originAccount.Id)).ReturnsAsync(originAccount);
        _accountRepositoryMock.Setup(x => x.GetByIdAsync(destAccount.Id)).ReturnsAsync(destAccount);

        var dto = new TransferDto(originAccount.Id, destAccount.Id, 100);

        await _transferService.PerformTransferAsync(dto);

        Assert.Equal(400, originAccount.Balance); 
        Assert.Equal(100, destAccount.Balance);   

        _accountRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Account>()), Times.Exactly(2));
    }
}