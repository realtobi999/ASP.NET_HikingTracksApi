﻿using HikingTracks.Application.Interfaces;
using HikingTracks.Domain;
using HikingTracks.Domain.DTO;
using HikingTracks.Domain.Entities;
using HikingTracks.Domain.Exceptions;
using HikingTracks.Domain.Interfaces;

namespace HikingTracks.Application.Service.Hikes;

public class HikeService : IHikeService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;

    public HikeService(IRepositoryManager repository, ILoggerManager logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Hike> CreateHike(CreateHikeDto createHikeDto)
    {
        var account = await _repository.Account.GetAccount(createHikeDto.AccountId) ?? throw new AccountNotFoundException(createHikeDto.AccountId); 

        var hike = new Hike{
            ID = createHikeDto.ID ?? Guid.NewGuid(),
            AccountId = account.ID,
            Title = createHikeDto.Title,
            Description = createHikeDto.Description,
            Distance = createHikeDto.Distance,
            ElevationGain = createHikeDto.ElevationGain,
            ElevationLoss = createHikeDto.ElevationLoss,
            // Calculate the average speed by dividing the distance and the total moving time
            AverageSpeed = createHikeDto.Distance / createHikeDto.MovingTime.TotalSeconds,
            MaxSpeed = createHikeDto.MaxSpeed,
            MovingTime = createHikeDto.MovingTime,
            Coordinates = createHikeDto.Coordinates,
            CreatedAt = DateTimeOffset.UtcNow
        };

        _repository.Hike.CreateHike(hike);

        // Update user total distance etc..
        account.UpdateAccountStatistics(hike); 

        await _repository.SaveAsync();

        return hike;
    }

    public async Task DeleteHike(Guid Id)
    {
        var hike = await _repository.Hike.GetHike(Id) ?? throw new HikeNotFoundException(Id);

        _repository.Hike.DeleteHike(hike);
        await _repository.SaveAsync();
    }

    public async Task<IEnumerable<Hike>> GetAllHikes()
    {
        var hikes = await _repository.Hike.GetAllHikes();

        return hikes;        
    }

    public async Task<IEnumerable<Hike>> GetAllHikesByAccount(Guid accountId)
    {
        var hikes = await _repository.Hike.GetAllHikesByAccount(accountId);

        return hikes;
    }

    public async Task<Hike> GetHike(Guid Id)
    {
        var hike = await _repository.Hike.GetHike(Id) ?? throw new HikeNotFoundException(Id);

        return hike;
    }

    public async Task<int> UpdateHike(Guid Id ,UpdateHikeDto updateHikeDto)
    {
        var hike = await _repository.Hike.GetHike(Id) ?? throw new HikeNotFoundException(Id);

        hike.Distance = updateHikeDto.Distance;
        hike.ElevationGain = updateHikeDto.ElevationGain;
        hike.ElevationLoss = updateHikeDto.ElevationLoss;
        hike.MaxSpeed = updateHikeDto.MaxSpeed;
        hike.MovingTime = updateHikeDto.MovingTime;
        hike.Kudos = updateHikeDto.Kudos;

        return await _repository.SaveAsync();
    }


    public async Task<int> UpdateHikePictures(Guid Id, IEnumerable<Photo> photos)
    {
        var hike = await _repository.Hike.GetHike(Id) ?? throw new HikeNotFoundException(Id);

        foreach (var photo in photos)
        {
            hike.Photos.Add(photo);
        }

        return await _repository.SaveAsync();
    }
}
