using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SpaceJam2.Models;

namespace SpaceJam2.Controllers
{
    public class GameController : Controller
    {
        private readonly SpaceJamDAL _SpaceJamDAL;
        
        private readonly SpaceJamContext _context;


        public GameController(SpaceJamContext context)
        {
            
            _SpaceJamDAL = new SpaceJamDAL();
            _context = context;

        }

        public IActionResult PlayGame(ToonSquad squad, Monstars monstars)
        {
            double squadStrength = GetSquadStrength(squad);
            double monastarStrength = GetMonstarsStrength(monstars);
            List<double> game = new List<double>() { squadStrength, monastarStrength };
            return View(game);
        }

        public double GetSquadStrength(ToonSquad team)
        {
            double totalStrength = 0;
            string[] playerIds = { team.Player1, team.Player2, team.Player3, team.Player4, team.Player5 };
            foreach(string p in playerIds) //goes through all the player ids and gets their stats/strength based off those stats and adds them together
            {
                List<PlayerStats> getplayer = _context.PlayerStats.Where(x => x.PlayerId == p).ToList();
                PlayerStats player = getplayer[0];
                double pointStrength = player.Points * 2.5;
                double assistStrength = player.Assists * 1.5;
                double reboundStrength = player.Rebounds * 1.5;
                double blockStrength = player.Blocks * 2;
                double stealStrength = player.Steals * 2;
                double playerStrength = pointStrength + assistStrength + reboundStrength + blockStrength + stealStrength;
                totalStrength += playerStrength;
            }
            return totalStrength;
        }

        public double GetMonstarsStrength(Monstars team)
        {
            double totalStrength = 0;
            string[] playerIds = { team.Blanko, team.Bupkus, team.Nawt, team.Pound, team.Bang};
            foreach (string p in playerIds) //goes through all the player ids and gets their stats/strength based off those stats and adds them together
            {
                List<PlayerStats> getplayer = _context.PlayerStats.Where(x => x.PlayerId == p).ToList();
                PlayerStats player = getplayer[0];
                double pointStrength = player.Points * 2.5;
                double assistStrength = player.Assists * 1.5;
                double reboundStrength = player.Rebounds * 1.5;
                double blockStrength = player.Blocks * 2;
                double stealStrength = player.Steals * 2;
                double playerStrength = pointStrength + assistStrength + reboundStrength + blockStrength + stealStrength;
                totalStrength += playerStrength;
            }
            return totalStrength;
        }
    }
}
