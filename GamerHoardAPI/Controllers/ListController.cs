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
    public class ListController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ListController(IConfiguration configuration)
        {
            _config = configuration;
        }

        [HttpGet("/api/Lists")]
        public async Task<ListsResultsDTO> GetAsync(int skip, int take, int userId, bool shared)
        {

            var db = new PostgresContext();

            var localDBResults = db.TbListHeaders.Skip(skip).Take(take).ToList();

            ListsResultsDTO listsResultsDTO = new ListsResultsDTO
            {
                results = localDBResults,
                totalResults = localDBResults.Count,
            };

            return listsResultsDTO;

        }
    }
}