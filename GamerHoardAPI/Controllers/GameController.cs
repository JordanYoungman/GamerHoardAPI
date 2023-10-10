using IGDB;
using Microsoft.AspNetCore.Mvc;
using IGDB;
using IGDB.Models;
using SteamWebAPI2.Utilities;
using SteamWebAPI2.Interfaces;
using Steam.Models.SteamCommunity;
using GamerHoardAPI.Models;

namespace GamerHoardAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        


    private readonly IConfiguration _config;

        public GameController(IConfiguration configuration)
        {
            _config = configuration;
        }

        [HttpGet("/api/Games")]
        public async Task<List<GameDTO>> GetAsync(int skip, int take, string steamId, string? gameToSearch = null)
        {

            var igdb = new IGDBClient(
            // Found in Twitch Developer portal for your app
            _config.GetValue<string>("IGDB_CLIENT_ID"),
            _config.GetValue<string>("IGDB_CLIENT_SECRET")
             );

            var webInterfaceFactory = new SteamWebInterfaceFactory(_config.GetValue<string>("STEAM_API_KEY"));

            var playerInterface = webInterfaceFactory.CreateSteamWebInterface<PlayerService>(new HttpClient());

            var steamOwnedGames = await playerInterface.GetOwnedGamesAsync(Convert.ToUInt64(steamId), true);

            var results = steamOwnedGames.Data.OwnedGames.Where(x => x.HasCommunityVisibleStats == true).ToList();

            IEnumerable<OwnedGameModel> ownedGames;

            if(gameToSearch != null)
            {
                ownedGames = results.Where(x => x.Name.ToLower().Contains(gameToSearch.ToLower())).Skip(skip).Take(take).ToList();
            } 
            else
            {
                ownedGames = results.Skip(skip).Take(take).ToList();
            }

            List<GameDTO> games = new List<GameDTO>();

            foreach (OwnedGameModel ownedGameModel in ownedGames)
            {
                var websiteFound = await igdb.QueryAsync<Website>(IGDBClient.Endpoints.Websites, $"fields id, game.id, category; where url = \"{"https://store.steampowered.com/app/" + ownedGameModel.AppId}\";");
                if(websiteFound.FirstOrDefault() != null)
                {
                    var valueToSearch = websiteFound.FirstOrDefault().Game.Value.Id;

                    var gamesFound = await igdb.QueryAsync<Game>(IGDBClient.Endpoints.Games, $"fields id, name, slug, checksum, category, storyline, created_at, first_release_date, genres.*, platforms.*, rating, rating_count, status, cover.*, themes.*, total_rating, total_rating_count, aggregated_rating, aggregated_rating_count, updated_at, url; where id = {valueToSearch};");
                    //cover ,first_release_date, franchise, franchises, genres, involved_companies, keywords, language_supports, name, platforms, rating, rating_count, slug, status, symmary, tags, themes, url, videos;");

                    if (gamesFound.Length > 0)
                    {
                        var game = gamesFound.FirstOrDefault();

                        GameDTO gameDTO = new GameDTO()
                        {
                            SourceSystem = SourceSystem.Steam,
                            Id = game.Id,
                            Name = game.Name,
                            Slug = game.Slug,
                            Checksum = game.Checksum,
                            Category = game.Category,
                            Storyline = game.Storyline,
                            Summary = game.Summary,
                            CreatedAt = game.CreatedAt,
                            FirstReleaseDate = game.CreatedAt,
                            Genres = game.Genres,
                            Covers = game.Cover,
                            Platforms = game.Platforms,
                            Rating = game.Rating,
                            RatingCount = game.RatingCount,
                            Status = game.Status,
                            Themes = game.Themes,
                            TotalRating = game.TotalRating,
                            TotalRatingCount = game.TotalRatingCount,
                            AggregatedRating = game.AggregatedRating,
                            AggregatedRatingCount = game.AggregatedRatingCount,
                            UpdatedAt = game.UpdatedAt,
                            Url = game.Url,
                        };
                        games.Add(gameDTO);
                    }
                }
                else
                {
                    var gamesFound = await igdb.QueryAsync<Game>(IGDBClient.Endpoints.Games, $"search \"{ownedGameModel.Name}\"; fields id, name, slug, checksum, category, storyline, created_at, first_release_date, genres.*, platforms.*, rating, rating_count, status, cover.*, themes.*, total_rating, total_rating_count, aggregated_rating, aggregated_rating_count, updated_at, url;");

                    if (gamesFound.Length > 0)
                    {
                        var game = gamesFound.FirstOrDefault();

                        GameDTO gameDTO = new GameDTO()
                        {
                            SourceSystem = SourceSystem.Steam,
                            Id = game.Id,
                            Name = game.Name,
                            Slug = game.Slug,
                            Checksum = game.Checksum,
                            Category = game.Category,
                            Storyline = game.Storyline,
                            Summary = game.Summary,
                            CreatedAt = game.CreatedAt,
                            FirstReleaseDate = game.CreatedAt,
                            Genres = game.Genres,
                            Covers = game.Cover,
                            Platforms = game.Platforms,
                            Rating = game.Rating,
                            RatingCount = game.RatingCount,
                            Status = game.Status,
                            Themes = game.Themes,
                            TotalRating = game.TotalRating,
                            TotalRatingCount = game.TotalRatingCount,
                            AggregatedRating = game.AggregatedRating,
                            AggregatedRatingCount = game.AggregatedRatingCount,
                            UpdatedAt = game.UpdatedAt,
                            Url = game.Url,
                        };
                        games.Add(gameDTO);
                    }
                }
            }

            return games;

        }
    }
}