using IGDB;
using Microsoft.AspNetCore.Mvc;
using IGDB;
using IGDB.Models;
using SteamWebAPI2.Utilities;
using SteamWebAPI2.Interfaces;
using Steam.Models.SteamCommunity;

namespace GamerHoardAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        


    private readonly IConfiguration _config;

        public WeatherForecastController(IConfiguration configuration)
        {
            _config = configuration;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<List<Game>> GetAsync()
        {

            var igdb = new IGDBClient(
            // Found in Twitch Developer portal for your app
            _config.GetValue<string>("IGDB_CLIENT_ID"),
            _config.GetValue<string>("IGDB_CLIENT_SECRET")
             );

            var webInterfaceFactory = new SteamWebInterfaceFactory(_config.GetValue<string>("STEAM_API_KEY"));

            var playerInterface = webInterfaceFactory.CreateSteamWebInterface<PlayerService>(new HttpClient());

            var results = await playerInterface.GetOwnedGamesAsync(Convert.ToUInt64("76561198129649478"), true, true);

            List<Game> games = new List<Game>();

            foreach(OwnedGameModel ownedGameModel in results.Data.OwnedGames)
            {
                var game = await igdb.QueryAsync<Game>(IGDBClient.Endpoints.Games, $"search \"{ownedGameModel.Name}\"; fields *;");
                //cover ,first_release_date, franchise, franchises, genres, involved_companies, keywords, language_supports, name, platforms, rating, rating_count, slug, status, symmary, tags, themes, url, videos;");

                    if (game.Length > 0)
                    {
                        games.Add(game.First());
                    }
            }

            return games;




            //var igdb = new IGDBClient(
            //// Found in Twitch Developer portal for your app
            //_config.GetValue<string>("IGDB_CLIENT_ID"),
            //_config.GetValue<string>("IGDB_CLIENT_SECRET")
            // );

            //var games = await igdb.QueryAsync<Game>(IGDBClient.Endpoints.Games, query: "fields id,name; where id = 4;");

            //return games;

        }
    }
}