﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SpaceJam2.Models;

namespace SpaceJam2.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly SpaceJamDAL _spaceJamDAL;
        private readonly SpaceJamContext _context;
        public HomeController(SpaceJamContext context)
        {
            _spaceJamDAL = new SpaceJamDAL();
            _context = context;
            
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
        [Authorize]
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
            int teamNumber = int.Parse(td);
            string activeUserId = GetActiveUser();

            Players p = await _spaceJamDAL.GetSpecificPlayer(id);

            Stats s = await _spaceJamDAL.GetStats(id);

            PlayerStats ps = new PlayerStats();

            ps.PlayerId = p.id.ToString();
            ps.PlayerName = p.first_name + " " + p.last_name.ToString();
            //ps.Points = s.pts;
            //ps.Assists = s.ast;
            //ps.Rebounds = s.oreb + s.dreb;
            //ps.Blocks = s.blk;
            //ps.Steals = s.stl;
            ps.Points = Math.Round(s.pts, 2);
            ps.Assists = Math.Round(s.ast, 2);
            ps.Rebounds = Math.Round(s.oreb + s.dreb, 2);
            ps.Blocks = Math.Round(s.blk, 2);
            ps.Steals = Math.Round(s.stl, 2);
            ToonSquad toonSquad3 = _context.ToonSquad.Find(teamNumber);
            //List<ToonSquad> toonSquad3 = _context.ToonSquad.Where(t => t.Id == i).ToList();
            toonSquad3.UserId = GetActiveUser();
            _context.Entry(toonSquad3).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.Update(toonSquad3);
            _context.SaveChanges();

            List<PlayerStats> checkForDupes = _context.PlayerStats.Where(c => c.PlayerId == ps.PlayerId).ToList();
            //List<ToonSquad> toonsquad = _spaceJamDAL.UserSelection.Where(ps => ps.)
            AddPlayer(id.ToString(),toonSquad3);
            if (checkForDupes.Count==0)
            {
                if (ModelState.IsValid)
                {
                    _context.PlayerStats.Add(ps);
                    _context.SaveChanges();
                }

                return RedirectToAction("ViewTeams");
            }
            else
            {
                ViewBag.Error = "This player is already on the Toon Squad playa!";
                return RedirectToAction("ViewTeams");
            }
        }
        [Authorize]
        public IActionResult RemovePlayer(string playerId)
        {
            string td = TempData["TeamNumber"].ToString();
            int teamNumber = int.Parse(td);
            ToonSquad toonSquad3 = _context.ToonSquad.Find(teamNumber);

            if (playerId == toonSquad3.Player1)
            {

                toonSquad3.Player1 = null;

            }
            else if (playerId == toonSquad3.Player2)
            {
                toonSquad3.Player2 = null;

            }
            else if (playerId == toonSquad3.Player3)
            {
                toonSquad3.Player3 = null;
            }
            else if (playerId == toonSquad3.Player4)
            {
                toonSquad3.Player4 = null;
            }
            else if (playerId == toonSquad3.Player5)
            {
                toonSquad3.Player5 = null;
            }

            _context.Entry(toonSquad3).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.Update(toonSquad3);
            _context.SaveChanges();

            return RedirectToAction("ViewTeams");
        }
        public void AddPlayer(string id, ToonSquad toonsquad2)
        {

            if (toonsquad2.Player1 == null)
            {
                toonsquad2.Player1 = id;

                _context.Entry(toonsquad2).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.Update(toonsquad2);
                _context.SaveChanges();
                //_supersdb.Entry(dbSuper).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                //_supersdb.Update(dbSuper);
                //_supersdb.SaveChanges();
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

        public IActionResult ViewTeams()
        {
            string user = GetActiveUser();
            Dictionary<ToonSquad, List<PlayerStats>> ToonSquadStats = new Dictionary<ToonSquad, List<PlayerStats>>();
            List<ToonSquad> userTeams = _context.ToonSquad.Where(u => u.UserId == user).ToList();
            for (int ut = 0; ut < userTeams.Count; ut++)
            {
                string[] playerids = { userTeams[ut].Player1, userTeams[ut].Player2, userTeams[ut].Player3, userTeams[ut].Player4, userTeams[ut].Player5 };
                
                List<PlayerStats> playerStats = GetPlayerStats(playerids);
                ToonSquadStats.Add(userTeams[ut], playerStats);
            }

            return View(ToonSquadStats);
        }
        public List<PlayerStats> GetPlayerStats(string[] playerIds)
        {
            List<PlayerStats> playerStats = new List<PlayerStats>();
            for (int id = 0; id < playerIds.Length; id++)
            {
                if (playerIds[id] == null)
                {
                    continue;
                }
                List<PlayerStats> getStats = _context.PlayerStats.Where(ps => ps.PlayerId == playerIds[id]).ToList();
                playerStats.Add(getStats[0]);
            }
            return (playerStats);

        }
        public IActionResult SetTeamNumber(int teamNumber)
        {
            TempData["TeamNumber"] = teamNumber;
          return  RedirectToAction("ViewTeams");
        }
    }
}
