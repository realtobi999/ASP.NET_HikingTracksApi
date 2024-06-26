﻿using HikingTracks.Domain;
using HikingTracks.Domain.Exceptions;
using HikingTracks.Domain.Interfaces;

namespace HikingTracks.Application.Service.Photos;

public class PhotoService : IPhotoService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;

    public PhotoService(IRepositoryManager repository, ILoggerManager logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Photo> CreatePhoto(CreatePhotoDto createPhotoDto)
    {
        var photo = new Photo(){
            ID = Guid.NewGuid(),
            HikeID = createPhotoDto.HikeID,
            FileName = createPhotoDto.FileName,
            Length = createPhotoDto.Length,
            Content = createPhotoDto.Content,
        };

        _repository.Photo.CreatePhoto(photo);
        await _repository.SaveAsync();

        return photo;
    }

    public async Task DeletePhoto(Guid Id)
    {
        var photo = await _repository.Photo.GetPhoto(Id) ?? throw new PhotoNotFoundException(Id);

        _repository.Photo.DeletePhoto(photo);
        await _repository.SaveAsync();
    }
}
