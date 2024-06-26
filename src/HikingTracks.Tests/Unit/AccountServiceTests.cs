﻿using FluentAssertions;
using HikingTracks.Application.Service.Accounts;
using HikingTracks.Domain.DTO;
using HikingTracks.Domain.Entities;
using HikingTracks.Domain.Exceptions;
using HikingTracks.Domain.Interfaces;
using HikingTracks.Tests.Integration.AccountEndpointTests;
using Moq;

namespace HikingTracks.Tests.Unit;

public class AccountServiceTests
{
    [Fact]
    public async void Account_GetAllAccount_Works()
    {
        var account1 = new Account().WithFakeData();
        var account2 = new Account().WithFakeData();
        var repository = new Mock<IRepositoryManager>();
        var logger = new Mock<ILoggerManager>();

        repository.Setup(repo => repo.Account.GetAllAccounts()).ReturnsAsync([account1, account2]);
        var service = new AccountService(repository.Object, logger.Object);

        var accounts = await service.GetAllAccounts();

        accounts.Should().NotBeEmpty();
        accounts.ElementAt(0).Should().BeEquivalentTo(account1);
        accounts.ElementAt(1).Should().BeEquivalentTo(account2);
    }

    [Fact]
    public async void Account_GetAccount_Works()
    {
        var account = new Account().WithFakeData();
        var repository = new Mock<IRepositoryManager>();
        var logger = new Mock<ILoggerManager>();
        repository.Setup(repo => repo.Account.GetAccount(account.ID)).ReturnsAsync(account);
        var service = new AccountService(repository.Object, logger.Object);

        var retrievedAccount = await service.GetAccount(account.ID);
    
        retrievedAccount.Should().NotBeNull();
        retrievedAccount.Should().BeEquivalentTo(account.ToDTO());
    }

    [Fact]
    public async void Account_GetAccount_FailsWhenNotFound()
    {
        var repository = new Mock<IRepositoryManager>();
        var logger = new Mock<ILoggerManager>();
        repository.Setup(repo => repo.Account.GetAccount(Guid.Empty)).ReturnsAsync(null as Account);
        var service = new AccountService(repository.Object, logger.Object);

        var account = await Assert.ThrowsAsync<AccountNotFoundException>(async () => await service.GetAccount(Guid.Empty));
    } 

    [Fact]
    public async void Account_DeleteAccount_FailsWhenNotFound()
    {
        var repository = new Mock<IRepositoryManager>();
        var logger = new Mock<ILoggerManager>();
        repository.Setup(repo => repo.Account.GetAccount(Guid.Empty)).ReturnsAsync(null as Account);
        var service = new AccountService(repository.Object, logger.Object);

        var account = await Assert.ThrowsAsync<AccountNotFoundException>(async () => await service.DeleteAccount(Guid.Empty));
    }

    [Fact]
    public async void Account_CreateAccount_Works()
    {
        // Prepare
        var createAccountDto = new Account().WithFakeData().ToCreateAccountDto();
        if (createAccountDto.ID is null) throw new Exception("Something went wrong... ID cannot be null");
        var repository = new Mock<IRepositoryManager>();
        var logger = new Mock<ILoggerManager>();

        repository.Setup(repo => repo.Account.CreateAccount(It.IsAny<Account>()));

        var service = new AccountService(repository.Object, logger.Object);

        // Act & Assert 
        var account = await service.CreateAccount(createAccountDto);

        account.Should().NotBeNull();
        account.ID.Should().Be((Guid) createAccountDto.ID);
        account.Email.Should().Be(createAccountDto.Email);
        account.Password.Should().Be(createAccountDto.Password);
        account.Username.Should().Be(createAccountDto.Username);
    }
}
