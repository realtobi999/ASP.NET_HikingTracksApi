﻿using System.ComponentModel.DataAnnotations;
using HikingTracks.Domain.Entities;

namespace HikingTracks.Domain.DTO;

public record class CreateHikeDto
{
    public Guid? ID { get; set; }

    [Required]
    public Guid AccountId { get; set; }

    [Required]
    public string? Title { get; set; }

    [Required]
    public string? Description { get; set; }
    
    [Required, Range(0, double.MaxValue)]
    public double Distance { get; set; }

    [Required, Range(0, double.MaxValue)]
    public double ElevationGain { get; set; }

    [Required, Range(0, double.MaxValue)]
    public double ElevationLoss { get; set; }

    [Required, Range(0, double.MaxValue)]
    public double MaxSpeed { get; set; }

    [Required]
    public TimeSpan MovingTime { get; set; }

    [Required]
    public List<Coordinate> Coordinates { get; set; } = [];
}