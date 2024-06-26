﻿using System.Net.Http.Json;
using FluentAssertions;
using HikingTracks.Domain;
using HikingTracks.Domain.DTO;
using HikingTracks.Domain.Entities;
using HikingTracks.Domain.Exceptions;
using HikingTracks.Presentation;
using Xunit.Sdk;

namespace HikingTracks.Tests.Integration.AccountEndpointTests;

public class AccountControllerTests
{
    [Fact]
    public async Task Account_GetAccounts_ReturnsEmptyWhenNoAccounts()
    {
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var response = await client.GetAsync("/api/account");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        (await response.Content.ReadFromJsonAsync<List<AccountDto>>()).Should().BeEmpty();
    }

    [Fact]
    public async Task Account_GetAccount_Works()
    {
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var account = new Account().WithFakeData();

        var create = await client.PostAsJsonAsync("/api/account", account.ToCreateAccountDto());
        create.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var response = await client.GetAsync(string.Format("/api/account/{0}", account.ID));

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var responseBody = await response.Content.ReadFromJsonAsync<AccountDto>() ?? throw new Exception("Failed to deserialize the response body into an AccountDto object.");
        responseBody.ID.Should().Be(account.ID);
    }

    [Fact]
    public async Task Account_CreateAccount_ReturnsCreated()
    {
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var account = new Account().WithFakeData();

        var response = await client.PostAsJsonAsync("/api/account", account.ToCreateAccountDto());

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        response.Headers.Contains("Location").Should().BeTrue();

        var header = response.Headers.GetValues("Location");

        header.Should().Equal(string.Format("/api/account/{0}", account.ID));
    }

    [Fact]
    public async Task Account_UpdateAccount_Works()
    {
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var account = new Account().WithFakeData();

        var updateAccountDto = new UpdateAccountDto
        {
            Username = "tobinek",
            Email = "tobiasfilgas@gmail.com",
        };

        var create = await client.PostAsJsonAsync("/api/account", account.ToCreateAccountDto());
        create.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var login = await client.PostAsJsonAsync("/api/login", account.ToLoginAccountDto());
        login.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var token = await login.Content.ReadFromJsonAsync<TokenDto>(); 
        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token?.Token));

        // Act
        var update = await client.PutAsJsonAsync(string.Format("/api/account/{0}", account.ID), updateAccountDto);
        update.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var response = await client.GetAsync(string.Format("/api/account/{0}", account.ID));
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        // Assert
        var body = await response.Content.ReadFromJsonAsync<AccountDto>() ?? throw new Exception("Failed to deserialize the response body into an AccountDto object.");
        body.Username.Should().Be("tobinek");
        body.Email.Should().Be("tobiasfilgas@gmail.com");
    }

    [Fact]
    public async Task Account_DeleteAccount_ReturnsOK()
    {
        // Prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var account = new Account().WithFakeData();

        var create = await client.PostAsJsonAsync("/api/account", account.ToCreateAccountDto());
        create.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var login = await client.PostAsJsonAsync("/api/login", account.ToLoginAccountDto());
        login.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var token = await login.Content.ReadFromJsonAsync<TokenDto>(); 
        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token?.Token));

        // Act & Assert
        var response = await client.DeleteAsync(string.Format("/api/account/{0}", account.ID));

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async Task Account_GetAccounts_LimitAndOffsetWorks()
    {
        // Prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var account1 = new Account().WithFakeData();
        var account2 = new Account().WithFakeData();
        var account3 = new Account().WithFakeData();

        var create1 = await client.PostAsJsonAsync("/api/account", account1.ToCreateAccountDto());
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var create2 = await client.PostAsJsonAsync("/api/account", account2.ToCreateAccountDto());
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var create3 = await client.PostAsJsonAsync("/api/account", account3.ToCreateAccountDto());
        create3.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // Act & Assert
        var limit = 2;
        var offset = 1;
        var response = await client.GetAsync(string.Format("/api/account?limit={0}&offset={1}", limit, offset));
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var body = await response.Content.ReadFromJsonAsync<List<AccountDto>>() ?? throw new Exception("Failed to deserialize the response body into an AccountDto object.");

        body.Should().NotBeEmpty();
        body.Count.Should().Be(limit);
        body.ElementAt(0).ID.Should().Be(account2.ID);
        body.ElementAt(1).ID.Should().Be(account3.ID);
    }
}
