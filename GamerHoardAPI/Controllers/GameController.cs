using IGDB;
using Microsoft.AspNetCore.Mvc;
using IGDB;
using IGDB.Models;
using SteamWebAPI2.Utilities;
using SteamWebAPI2.Interfaces;
using Steam.Models.SteamCommunity;
using GamerHoardAPI.Models;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using SteamWebAPI2.Mappings;
using SteamStorefrontAPI.Classes;
using SteamStorefrontAPI;

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
        public async Task<List<SteamApp>> GetAsync(int skip, int take, string steamId, string? gameToSearch = null)
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

            List<SteamApp> steamGamesOwned = new List<SteamApp>();

            foreach (OwnedGameModel ownedGameModel in ownedGames)
            {
                SteamApp steamApp = await AppDetails.GetAsync(Convert.ToInt32(ownedGameModel.AppId));
                if(steamApp != null)
                {
                    steamGamesOwned.Add(steamApp);
                }
                else
                {
                    Console.WriteLine(ownedGameModel);
                }
            }

            return steamGamesOwned;

        }
    }
}