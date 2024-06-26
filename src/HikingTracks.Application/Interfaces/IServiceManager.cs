﻿using HikingTracks.Application.Interfaces;

namespace HikingTracks.Application.Interfaces;

public interface IServiceManager
{
    public IAccountService AccountService { get; }
    public IHikeService HikeService { get; }
    public IPhotoService PhotoService { get; }
    public IFormFileService FormFileService { get; }
    public ITokenService TokenService { get; }
    public ISegmentService SegmentService { get; }
    public ISegmentHikeService SegmentHikeService { get; }
}
