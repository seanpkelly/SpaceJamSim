using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SpaceJam2.Models;

namespace SpaceJam2.Controllers
{
    public class MonstarController : Controller
    {
        private readonly SpaceJamDAL _spaceJamDAL;
        private readonly SpaceJamContext _context;
        public MonstarController(SpaceJamContext context)
        {
            _spaceJamDAL = new SpaceJamDAL();
            _context = context;
        }
        public IActionResult GenerateMonstars()
        {
            Monstars monstars = new Monstars();
            List<PlayerStats> allPlayers = _context.PlayerStats.ToList();
            int range = allPlayers.Count();

            List<int> indexes = new List<int>();
            for (int i = 0; indexes.Count < 5; i++)
            {
                int index = RandomNumber(range);
                if (!indexes.Contains(index))
                {
                    indexes.Add(index);
                }
            }
            monstars.Bang = allPlayers[indexes[0]].PlayerId.ToString();
            monstars.Pound = allPlayers[indexes[1]].PlayerId.ToString();
            monstars.Nawt = allPlayers[indexes[2]].PlayerId.ToString();
            monstars.Blanko = allPlayers[indexes[3]].PlayerId.ToString();
            monstars.Bupkus = allPlayers[indexes[4]].PlayerId.ToString();
            //int index = RandomNumber(range);
            //monstars.Bang = allPlayers[index].PlayerId;
            //index = RandomNumber(range);
            //monstars.Blanko = allPlayers[index].PlayerId;
            //index = RandomNumber(range);
            //monstars.Bupkus = allPlayers[index].PlayerId;
            //index = RandomNumber(range);
            //monstars.Nawt = allPlayers[index].PlayerId;
            //index = RandomNumber(range);
            //monstars.Pound = allPlayers[index].PlayerId;

            _context.Monstars.Add(monstars);
            _context.SaveChanges();
            return RedirectToAction("PlayGame","Game", monstars);
        }
        public int RandomNumber(int number)
        {
            Random random1 = new Random();
            int random = random1.Next(1, number);
            return random;
        }
    }
}
