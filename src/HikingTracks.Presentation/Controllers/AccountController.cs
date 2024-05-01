﻿using HikingTracks.Application.Interfaces;
using HikingTracks.Domain.DTO;
using Microsoft.AspNetCore.Mvc;

namespace HikingTracks.Presentation;

[Route("api/account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IServiceManager _service;

    public AccountController(IServiceManager service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAccounts()
    {
        var accounts = await _service.AccountService.GetAllAccounts();

        return Ok(accounts);
    }

    [HttpGet("{accountID:guid}")]
    public async Task<IActionResult> GetAccount(Guid accountID) 
    {
        var account = await _service.AccountService.GetAccount(accountID);

        if (account is null)
        {
            return NotFound();
        }

        return Ok(account);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountDto createAccountDto)
    {
        if (createAccountDto is null) 
        {
            return BadRequest("Body is not provided");
        }

        var account = await _service.AccountService.CreateAccount(createAccountDto);

        return Created(string.Format("/api/account/{0}", account.ID), account);
    }
}