﻿namespace HikingTracks.Domain.Interfaces;

public interface IRepositoryManager
{
    IAccountRepository Account { get; }
    IHikeRepository Hike { get; }
    IPhotoRepository Photo { get; }
    ISegmentRepository Segment { get; }
    ISegmentHikeRepository SegmentHike { get; }
    Task<int> SaveAsync();
}
