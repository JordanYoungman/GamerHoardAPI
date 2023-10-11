using IGDB.Models;
using IGDB;
using Steam.Models.SteamCommunity;

namespace GamerHoardAPI.Models
{
    public class GamesToChooseDTO
    {
        public List<GameDTO> GameChoices { get; set; } = new List<GameDTO>();

        public GameDTO? GameChoice { get; set; }
    }
}
