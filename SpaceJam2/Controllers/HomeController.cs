using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SpaceJam2.Models;

namespace SpaceJam2.Controllers
{

    public class HomeController : Controller
    {
        private readonly SpaceJamDAL _spaceJamDAL;
        private readonly SpaceJamContext _context;
        public HomeController()
        {
            _spaceJamDAL = new SpaceJamDAL();
        }
        public async Task<IActionResult> Index()
        {
            int ids = 200;
            Stats stats = await _spaceJamDAL.GetStats(ids);
            Players players = await _spaceJamDAL.GetSpecificPlayer(237);
            return View(players);
        }

        public string GetActiveUser()
        {
            string activeUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            return activeUserId;
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

        #region Players CRUD
        public async Task<IActionResult> AddToToonSquad(int id)
        {
            if (TempData["TeamNumber"] == null)
            {
                ToonSquad toonSquad = new ToonSquad();
                _context.ToonSquad.Add(toonSquad);
                _context.SaveChanges();
                TempData["TeamNumber"] = 1;
            }
            string td = TempData["TeamNumber"].ToString();
            int i = int.Parse(td);
            string activeUserId = GetActiveUser();

            Players p = await _spaceJamDAL.GetSpecificPlayer(id);

            Stats s = await _spaceJamDAL.GetStats(id);

            PlayerStats ps = new PlayerStats();

            ps.PlayerId = p.id.ToString();
            ps.PlayerName = p.first_name + p.last_name.ToString();
            ps.Points = s.pts;
            ps.Assists = s.ast;
            ps.Rebounds = s.oreb + s.dreb;
            ps.Blocks = s.blk;
            ps.Steals = s.stl;
            List<ToonSquad> toonSquad3 = _context.ToonSquad.Where(t => t.Id == i).ToList();
            
            List<PlayerStats> checkForDupes = _context.PlayerStats.Where(c => c.PlayerId == ps.PlayerId).ToList();
            //List<ToonSquad> toonsquad = _spaceJamDAL.UserSelection.Where(ps => ps.)

            if (checkForDupes == null)
            {
                if (ModelState.IsValid)
                {
                    _context.PlayerStats.Add(ps);
                    _context.SaveChanges();
                }

                return RedirectToAction("DisplayTeam");
            }
            else
            {
                ViewBag.Error = "This player is already on the Toon Squad playa!";
                return RedirectToAction();
            }
        }

        public void AddPlayer(string id, ToonSquad toonsquad2)
        {

            if (toonsquad2.Player1 == null)
            {
                toonsquad2.Player1 = id;
            }
            else if (toonsquad2.Player2 == null)
            {
                toonsquad2.Player2 = id;
            }
            else if (toonsquad2.Player3 == null)
            {
                toonsquad2.Player3 = id;
            }
            else if (toonsquad2.Player4 == null)
            {
                toonsquad2.Player4 = id;
            }
            else if (toonsquad2.Player5 == null)
            {
                toonsquad2.Player5 = id;
            }

        }
        #endregion
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
