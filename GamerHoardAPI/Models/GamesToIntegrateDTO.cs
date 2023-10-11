using IGDB.Models;
using IGDB;
using Steam.Models.SteamCommunity;

namespace GamerHoardAPI.Models
{
    public class GamesToIntegrateDTO
    {
        public List<GameDTO> AutoAddedGames { get; set; } = new List<GameDTO>();

        public List<GameDTO> ChooseFromGames { get; set; } = new List<GameDTO>();

        public List<OwnedGameModel> IssuesWithGames = new List<OwnedGameModel>();
    }
}
