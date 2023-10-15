using IGDB;
using Microsoft.AspNetCore.Mvc;
using IGDB;
using IGDB.Models;
using SteamWebAPI2.Utilities;
using SteamWebAPI2.Interfaces;
using Steam.Models.SteamCommunity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using SteamWebAPI2.Mappings;
using SteamStorefrontAPI.Classes;
using SteamStorefrontAPI;
using GamerHoardAPI.Models;
using System.Linq;
using craftersmine.SteamGridDBNet;
using static System.Net.WebRequestMethods;
using GamerHoardAPI.DTOs;

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
        public async Task<GameResultsDTO> GetAsync(int skip, int take, string steamId, string? gameToSearch = null)
        {

            var db = new PostgresContext();

            var igdb = new IGDBClient(
            // Found in Twitch Developer portal for your app
            _config.GetValue<string>("IGDB_CLIENT_ID"),
            _config.GetValue<string>("IGDB_CLIENT_SECRET")
             );

            var webInterfaceFactory = new SteamWebInterfaceFactory(_config.GetValue<string>("STEAM_API_KEY"));

            var playerInterface = webInterfaceFactory.CreateSteamWebInterface<PlayerService>(new HttpClient());

            var steamOwnedGames = await playerInterface.GetOwnedGamesAsync(Convert.ToUInt64(steamId), true);

            var steamOwnedGamesResults = steamOwnedGames.Data.OwnedGames.Where(x => x.HasCommunityVisibleStats == true).ToList();

            var localDBResults = db.TbGameHeaders.Where(x => steamOwnedGamesResults.Select(x => x.AppId).ToList().Contains((uint)x.AppId)).ToList();

            var steamOwnedGamesMap = new HashSet<int>(steamOwnedGamesResults.Select(x => (int)x.AppId).ToList());
            var localDBMap = new HashSet<int>(localDBResults.Select(x => (int)x.AppId).ToList());
            var differenceBetweenMaps = steamOwnedGamesMap.Except(localDBMap);

            foreach(var appID in differenceBetweenMaps)
            {
                SteamApp steamApp = await AppDetails.GetAsync(appID);

                Console.WriteLine(steamApp);
                
                if(steamApp != null)
                {
                    TbGameHeader newGame = new TbGameHeader
                    {
                        AppId = steamApp.SteamAppId,
                        Name = steamApp.Name,
                        ReleaseDate = steamApp.ReleaseDate.ToString(),
                        HeaderImage = steamApp.HeaderImage.ToString(),
                        Developers = steamApp.Developers.ToString(),
                        Publishers = steamApp.Publishers.ToString(),
                        Categories = steamApp.Categories.ToString(),
                        Genres = steamApp.Genres.ToString(),
                        Tags = "No Tags"
                    };

                    db.Add(newGame);
                    db.SaveChanges();
                }
                else
                {
                    var item = steamOwnedGamesResults.Where(y => y.AppId == (uint)appID).FirstOrDefault();
                    var slug = (item.Name.ToLower()).Replace(" ", "-").Replace(":", "");

                    var games = await igdb.QueryAsync<IGDB.Models.Game>(IGDBClient.Endpoints.Games, $"search \"{item.Name}\"; fields *;");
                    var game = games.Where(x => x.Slug == slug).FirstOrDefault();

                    if(game != null)
                    {
                        TbGameHeader newGame = new TbGameHeader
                        {
                            AppId = (int)item.AppId,
                            Name = item.Name,
                            ReleaseDate = game.ReleaseDates.ToString(),
                            HeaderImage = "https://cdn.akamai.steamstatic.com/steam/apps/" + item.AppId + "/header.jpg",
                            Developers = game.InvolvedCompanies.ToString(),
                            Publishers = game.InvolvedCompanies.ToString(),
                            Categories = game.Category.ToString(),
                            Genres = game.Genres.ToString(),
                            Tags = "No Tags"
                        };

                        db.Add(newGame);
                        db.SaveChanges();
                    }
                }
            }

            if(gameToSearch == null)
            {
                GameResultsDTO gameResultsDTO = new GameResultsDTO
                {
                    results = localDBResults.ToList().OrderBy(x => x.Name).Skip(skip).Take(take).ToList(),
                    totalResults = localDBResults.Count,
                };

                return gameResultsDTO;

            } else
            {
                GameResultsDTO gameResultsDTO = new GameResultsDTO
                {
                    results = localDBResults.ToList().OrderBy(x => x.Name).Where(x => x.Name.ToLower().Contains(gameToSearch.ToLower())).Skip(skip).Take(take).ToList(),
                    totalResults = localDBResults.Where(x => x.Name.ToLower().Contains(gameToSearch.ToLower())).ToList().Count,
                };

                return gameResultsDTO;
            }

        }
    }
}