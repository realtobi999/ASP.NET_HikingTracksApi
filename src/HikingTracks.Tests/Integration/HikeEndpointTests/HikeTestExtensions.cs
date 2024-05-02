﻿using Bogus;
using HikingTracks.Domain.Entities;

namespace HikingTracks.Tests;

public static class HikeTestExtensions
{
    private static readonly Faker<Hike> _hikeFaker = new Faker<Hike>()
            .RuleFor(h => h.ID, f => f.Random.Guid())
            .RuleFor(h => h.AccountID, f => f.Random.Guid())
            .RuleFor(h => h.Distance, f => f.Random.Double(0, 100))
            .RuleFor(h => h.ElevationGain, f => f.Random.Double(0, 500))
            .RuleFor(h => h.ElevationLoss, f => f.Random.Double(0, 500))
            .RuleFor(h => h.AverageSpeed, f => f.Random.Double(0, 20))
            .RuleFor(h => h.MaxSpeed, f => f.Random.Double(0, 30))
            .RuleFor(h => h.MovingTime, f => f.Date.Timespan())
            .RuleFor(h => h.Coordinates, f => GenerateRandomCoordinates());

    private static List<Coordinate> GenerateRandomCoordinates()
    {
        var random = new Random();
        var coordinates = new List<Coordinate>();

        for (int i = 0; i < 5; i++)
        {
            var latitude = random.NextDouble() * (90 - (-90)) + (-90);
            var longitude = random.NextDouble() * (180 - (-180)) + (-180);
            coordinates.Add(new Coordinate(latitude, longitude));
        }

        return coordinates;
    }

    public static Hike WithFakeData(this Hike hike)
    {
        return _hikeFaker.Generate();
    }
}