﻿using System.Net.Http.Json;
using FluentAssertions;
using HikingTracks.Domain;
using HikingTracks.Domain.Entities;
using HikingTracks.Presentation;
using HikingTracks.Tests.Integration.AccountEndpointTests;

namespace HikingTracks.Tests.Integration.Middleware;

public class MiddlewareTests
{
    [Fact]
    public async Task App_TestThatGlobalErrorHandlingWorksAsync()
    {
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var response = await client.GetAsync(string.Format("/api/account/{0}", Guid.Empty));

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        var body = await response.Content.ReadFromJsonAsync<ErrorDetails>() ?? throw new Exception("Failed to deserialize the response body into an AccountDto object.");

        body.StatusCode.Should().Be(404);
        body.Message.Should().Be("The account with the id: 00000000-0000-0000-0000-000000000000 doesn't exist.");
    }

    [Fact]
    public async Task AccountMiddleware_TestThatItReturns401()
    {
        // Prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var account1 = new Account().WithFakeData();
        var account2 = new Account().WithFakeData();

        var create1 = await client.PostAsJsonAsync("/api/account", account1.ToCreateAccountDto());
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var create2 = await client.PostAsJsonAsync("/api/account", account2.ToCreateAccountDto());
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var login = await client.PostAsJsonAsync("/api/login", account1.ToLoginAccountDto());
        login.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var token = await login.Content.ReadFromJsonAsync<TokenDto>() ?? throw new Exception("Failed to deserialize the response body.");

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token.Token));
        
        // Act & Assert
        // Use the account2.ID while having the token of the account1
        var response = await client.DeleteAsync(string.Format("/api/account/{0}", account2.ID));
        
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task HikeMiddleware_TestThatItReturns401()
    {
        // Prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var account1 = new Account().WithFakeData();
        var account2 = new Account().WithFakeData();
        var hike = new Hike().WithFakeData(account1);

        var create1 = await client.PostAsJsonAsync("/api/account", account1.ToCreateAccountDto());
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var create2 = await client.PostAsJsonAsync("/api/account", account2.ToCreateAccountDto());
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // Authenticate the request and create the hike with the owner being account1
        var login1 = await client.PostAsJsonAsync("/api/login", account1.ToLoginAccountDto());
        login1.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var token1 = await login1.Content.ReadFromJsonAsync<TokenDto>();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token1!.Token);

        var create3 = await client.PostAsJsonAsync("/api/hike", hike.ToCreateHikeDto());
        create3.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // Now try to delete hike authenticated as account2
        client.DefaultRequestHeaders.Remove("Authorization");
        
        var login2 = await client.PostAsJsonAsync("/api/login", account2.ToLoginAccountDto());
        login2.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var token2 = await login2.Content.ReadFromJsonAsync<TokenDto>();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token2!.Token);

        // Act & Assert
        var response = await client.DeleteAsync(string.Format("/api/hike/{0}", hike.ID));
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }
}
