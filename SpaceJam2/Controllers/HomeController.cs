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
            Players players = await _spaceJamDAL.GetSpecificPlayer(237);
            return View(players);
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
        //public async IActionResult AddToToonSquad(int id)
        //{
        //    //string activeUserId = GetActiveUser();

        //    Players p = await _spaceJamDAL.GetPlayers(id);

        //    p.id = id;
        //    p.first_name = "";
        //    p.last_name = "";
        //    p.position = "";
        //    p.team = 


        //    //remove game from history list if its added to favorites
        //    DeleteHistory(id);
        //    DeleteWishlist(id);

        //    //check for dupes does not throw an error message or return to search results correctly yet
        //    UserFavorite checkForDupes = _gameContext.UserFavorite.Where(f => f.UserId == activeUserId && f.GameId == id).FirstOrDefault();

        //    if (checkForDupes == null)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            _gameContext.UserFavorite.Add(f);
        //            _gameContext.SaveChanges();
        //        }

        //        ////iterate favorite counter here///////////////////////////////////////////////////////

        //        List<UserFavorite> favorite = _gameContext.UserFavorite.Where(f => f.GameId == id).ToList();
        //        int count = favorite.Max(m => m.FavoriteCount) + 1;

        //        foreach (var fav in favorite)
        //        {
        //            fav.FavoriteCount = count;
        //            _gameContext.Entry(fav).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        //            _gameContext.Update(fav);
        //            _gameContext.SaveChanges();
        //        }

        //        return RedirectToAction("DisplayFavorites");
        //    }
        //    else
        //    {
        //        ViewBag.Error = "This game is already a favorite!";
        //        return RedirectToAction("SearchResults");
        //    }

        //}
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
