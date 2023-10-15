using GamerHoardAPI.Models;

namespace GamerHoardAPI.DTOs
{
    public class GameResultsDTO
    {
        public List<TbGameHeader>? results { get; set; }
        public int totalResults {get; set;}
    }
}
