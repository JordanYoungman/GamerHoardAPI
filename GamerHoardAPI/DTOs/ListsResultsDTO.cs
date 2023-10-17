using GamerHoardAPI.Models;

namespace GamerHoardAPI.DTOs
{
    public class ListsResultsDTO
    {
        public List<TbListHeader>? results { get; set; }
        public int totalResults {get; set;}
    }
}
