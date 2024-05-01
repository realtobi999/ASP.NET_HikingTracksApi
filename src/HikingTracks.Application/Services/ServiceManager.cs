﻿using HikingTracks.Application.Interfaces;
using HikingTracks.Application.Services.AccountService;
using HikingTracks.Application.Services.HikeService;
using HikingTracks.Domain.Interfaces;

namespace HikingTracks.Application;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<IAccountService> _accountService;
    private readonly Lazy<IHikeService> _hikeService;

    public ServiceManager(IRepositoryManager repositoryManager, ILoggerManager logger)
    {
        _accountService = new Lazy<IAccountService>(() => new AccountService(repositoryManager, logger));
        _hikeService = new Lazy<IHikeService>(() => new HikeService(repositoryManager, logger));
    }

    public IAccountService AccountService => _accountService.Value;
    public IHikeService HikeService => _hikeService.Value;
}
