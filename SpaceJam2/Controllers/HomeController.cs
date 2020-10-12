using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SpaceJam2.Models;

namespace SpaceJam2.Controllers
{

    public class HomeController : Controller
    {
        private readonly SpaceJamDAL _spaceJamDAL;
        public HomeController()
        {
            _spaceJamDAL = new SpaceJamDAL();
        }
        public async Task<IActionResult> Index()
        {
            int ids = 200;
            Stats stats = await _spaceJamDAL.GetStats(ids);
            return View();
        }
        //public IActionResult Paginate()
        public async Task<IActionResult> PlayerList(int page, string search)
        {
            ViewBag.Name = search;
            if (page == 0)
            {
                page = 1;
            }
            int nextpage = page + 1;
            Search next = new Search();
            next.Name = search;
            next.Page = nextpage;
            ViewBag.NextPage = nextpage;
            if (page != 0)
            {
                ViewBag.PreviousPage = page - 1;
            }
            Stats blankStats = new Stats();
            List<Players> players = await _spaceJamDAL.GetPlayers(page, search);
            List<Stats> getStats = new List<Stats>();
            for (int i = 0; i < players.Count; i++)
            {
                int id = players[i].id;
                Stats statList = await _spaceJamDAL.GetStats(id);
                if (statList != null)
                {
                    getStats.Add(statList);
                }
                else
                {
                    getStats.Add(blankStats);
                }
            }
            PlayerStatViewModel playerStats = new PlayerStatViewModel();
            playerStats.players = players;
            playerStats.stats = getStats;
            return View(playerStats);
        }

        public IActionResult PlayerSearch()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
