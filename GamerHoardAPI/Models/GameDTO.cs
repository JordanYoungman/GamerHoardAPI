using IGDB.Models;
using IGDB;

namespace GamerHoardAPI.Models
{
    public class GameDTO
    {
            public SourceSystem SourceSystem { get; set; }
            public long? Id { get; set; }

            public string Name { get; set; }

            public string Slug { get; set; }

            public string Checksum { get; set; }

            public Category? Category { get; set; }

            public string Storyline { get; set; }

            public string Summary { get; set; }

            public DateTimeOffset? CreatedAt { get; set; }

            public DateTimeOffset? FirstReleaseDate { get; set; }

            public IdentitiesOrValues<Genre> Genres { get; set; }

            public IdentitiesOrValues<Platform> Platforms { get; set; }

            public double? Rating { get; set; }

            public int? RatingCount { get; set; }

            public GameStatus? Status { get; set; }

            public IdentitiesOrValues<Theme> Themes { get; set; }

            public double? TotalRating { get; set; }

            public int? TotalRatingCount { get; set; }

            public double? AggregatedRating { get; set; }

            public int? AggregatedRatingCount { get; set; }

            public DateTimeOffset? UpdatedAt { get; set; }

            public string Url { get; set; }

    }

    public enum SourceSystem
    {
        None = 0,
        Steam = 1,
    }
}
