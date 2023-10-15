using System;
using System.Collections.Generic;

namespace GamerHoardAPI.Models;

public partial class Game
{
    public int? AppId { get; set; }

    public string? Name { get; set; }

    public string? ReleaseDate { get; set; }

    public string? EstimatedOwners { get; set; }

    public int? PeakCcu { get; set; }

    public int? RequiredAge { get; set; }

    public double? Price { get; set; }

    public int? DlcCount { get; set; }

    public string? AboutTheGame { get; set; }

    public string? SupportedLanguages { get; set; }

    public string? FullAudioLanguages { get; set; }

    public string? Reviews { get; set; }

    public string? HeaderImage { get; set; }

    public string? Website { get; set; }

    public string? SupportUrl { get; set; }

    public string? SupportEmail { get; set; }

    public bool? Windows { get; set; }

    public bool? Mac { get; set; }

    public bool? Linux { get; set; }

    public int? MetacriticScore { get; set; }

    public string? MetacriticUrl { get; set; }

    public int? UserScore { get; set; }

    public int? Positive { get; set; }

    public int? Negative { get; set; }

    public string? ScoreRank { get; set; }

    public int? Achievements { get; set; }

    public int? Recommendations { get; set; }

    public string? Notes { get; set; }

    public int? AveragePlaytimeForever { get; set; }

    public int? AveragePlaytimeTwoWeeks { get; set; }

    public int? MedianPlaytimeForever { get; set; }

    public int? MedianPlaytimeTwoWeeks { get; set; }

    public string? Developers { get; set; }

    public string? Publishers { get; set; }

    public string? Categories { get; set; }

    public string? Genres { get; set; }

    public string? Tags { get; set; }

    public string? Screenshots { get; set; }

    public string? Movies { get; set; }
}
