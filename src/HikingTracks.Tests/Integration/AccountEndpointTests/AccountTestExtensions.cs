﻿using Bogus;
using HikingTracks.Domain;
using HikingTracks.Domain.DTO;
using HikingTracks.Domain.Entities;

namespace HikingTracks.Tests.Integration.AccountEndpointTests;

public static class AccountTestExtensions
{
    private static readonly Faker<Account> _accountFaker = new Faker<Account>()
    .RuleFor(a => a.ID, f => f.Random.Guid())
    .RuleFor(a => a.Username, f => f.Internet.UserName())
    .RuleFor(a => a.Email, f => f.Internet.Email())
    .RuleFor(a => a.Password, f => f.Internet.Password())
    .RuleFor(a => a.TotalHikes, f => f.Random.Int(0, 1000))
    .RuleFor(a => a.TotalDistance, f => f.Random.Double(0, 10000))
    .RuleFor(a => a.TotalMovingTime, f => f.Date.Timespan())
    .RuleFor(a => a.CreatedAt, f => f.Date.PastOffset());

    public static Account WithFakeData(this Account account)
    {
        return _accountFaker.Generate();
    }

    public static CreateAccountDto ToCreateAccountDto(this Account account)
    {
        return new CreateAccountDto{
            ID = account.ID,
            Username = account.Username,
            Email = account.Email,
            Password = account.Password,
        };
    }

    public static LoginAccountDto ToLoginAccountDto(this Account account)
    {
        return new LoginAccountDto{
            Email = account.Email,
            Password = account.Password
        };
    }
}
